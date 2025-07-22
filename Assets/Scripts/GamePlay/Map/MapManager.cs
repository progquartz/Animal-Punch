using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    public EnemySpawner EnemySpawner;
    public Transform MapModelParent;

    private Dictionary<Vector2Int, MapChunk> activeChunks = new Dictionary<Vector2Int, MapChunk>();
    public Vector2Int currentCenterChunk;
    private bool IsFirstTimeInitializing = true;

    public static event Action OnMapManagerInitialized;

    private const float chunkSize = 90f;
    private const float halfCellSize = chunkSize * 0.5f;
    private Vector3 lastPlayerPosition;
    private const float minDistanceToUpdate = 5f;

    protected override async void Init()
    {
        base.Init();
        IsDestroyOnLoad = true;

        if (EnemySpawner == null)
        {
            EnemySpawner = GetComponent<EnemySpawner>();
            if (EnemySpawner == null)
            {
                Debug.LogError("[MapManager] EnemySpawner가 없습니다. Init 중단.");
                return;
            }
        }

        await Task.Yield();

        Debug.Log("[MapManager] 적 풀링 준비 시작");
        await EnemySpawner.PreloadAllEnemyObjects();
        Debug.Log("[MapManager] 적 풀링 준비 끝");

        Debug.Log("[MapManager] 맵오브젝트 풀링 준비 시작");
        await MapObjectPool.Instance.PreloadAllMapObjects(10);
        Debug.Log("[MapManager] 맵오브젝트 풀링 준비 끝");

        InitializeNearbyBlock();
        Debug.Log("[MapManager] init 완료");

        OnMapManagerInitialized?.Invoke();
    }



    private void Update()
    {
        if (SceneLoader.Instance.CurrentScene == "Scenes/GameScene")
        {
            if (GameManager.Instance.IsGameStarted && !GameManager.Instance.IsGamePaused && !GameManager.Instance.IsGameOver)
            {
                if (Vector3.Distance(Player.Instance.PlayerTransform.position, lastPlayerPosition) > minDistanceToUpdate)
                {
                    Vector2Int playerChunkPos = CalculateCurrentPlayerChunkPos();
                    UpdateCenterChunk(playerChunkPos);
                    lastPlayerPosition = Player.Instance.PlayerTransform.position;
                }
            }
        }
    }

    private void InitializeNearbyBlock()
    {
        currentCenterChunk = Vector2Int.zero;
        UpdateCenterChunk(currentCenterChunk);
    }

    private Vector2Int CalculateCurrentPlayerChunkPos()
    {
        int gridX = Mathf.FloorToInt((Player.Instance.PlayerTransform.position.x + halfCellSize) / chunkSize);
        int gridZ = Mathf.FloorToInt((Player.Instance.PlayerTransform.position.z + halfCellSize) / chunkSize);

        return new Vector2Int(gridX, gridZ);
    }

    public void LoadChunksAround(Vector2Int center)
    {
        var mapDataStorage = DataManager.Instance.MapDataStorage;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkPos = new Vector2Int(center.x + x, center.y + y);

                if (!activeChunks.ContainsKey(chunkPos))
                {
                    Vector3 spawnPosition = new Vector3(chunkPos.x * mapDataStorage.chunkSize, 0, chunkPos.y * mapDataStorage.chunkSize);
                    GameObject chunkObj = Instantiate(mapDataStorage.GetRandomMapChunk(chunkPos).gameObject, spawnPosition, Quaternion.identity, MapModelParent);

                    MapChunk newChunk = chunkObj.GetComponent<MapChunk>();
                    newChunk.Initialize(chunkPos, mapDataStorage);
                    activeChunks.Add(chunkPos, newChunk);
                }
            }
        }
    }

    public void UpdateCenterChunk(Vector2Int newCenter)
    {
        if (IsFirstTimeInitializing || newCenter != currentCenterChunk)
        {
            IsFirstTimeInitializing = false;
            currentCenterChunk = newCenter;
            LoadChunksAround(currentCenterChunk);
            UnloadChunksNotNearCenter(currentCenterChunk);
        }
    }

    private void UnloadChunksNotNearCenter(Vector2Int center)
    {
        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in activeChunks)
        {
            if (Vector2Int.Distance(kvp.Key, center) > 1.5f)
            {
                kvp.Value.UnloadChunk();
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            activeChunks.Remove(key);
        }
    }
}
