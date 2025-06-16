using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    public EnemySpawner EnemySpawner;
    public Transform MapModelParent;

    // 현재 활성화된 청크들을 좌표를 키로 저장
    private Dictionary<Vector2Int, MapChunk> activeChunks = new Dictionary<Vector2Int, MapChunk>();

    public Vector2Int currentCenterChunk;
    private bool IsFirstTimeInitializing = true;
    private const float chunkSize = 90f;
    private const float halfCellSize = chunkSize * 0.5f;

    // 잦은 플레이어 위치 업데이트를 막기 위해 일정 이동거리 이상 지났을 때마다 업데이트 여부를 확인.
    private Vector3 lastPlayerPosition;
    private const float minDistanceToUpdate = 5f; // 최소 이동거리 설정

    protected override void Init()
    {
        IsDestroyOnLoad = true;
        base.Init();
        EnemySpawner.Init();
        InitializeNearbyBlock();
        Debug.Log("MapManager Init");
    }


    private void Update()
    {
        if (Vector3.Distance(Player.Instance.PlayerTransform.position, lastPlayerPosition) > minDistanceToUpdate)
        {
            Vector2Int playerChunkPos = CalculateCurrentPlayerChunkPos();
            UpdateCenterChunk(playerChunkPos);
            lastPlayerPosition = Player.Instance.PlayerTransform.position;
        }
    }


    private void InitializeNearbyBlock()
    {
        currentCenterChunk = Vector2Int.zero;
        UpdateCenterChunk(currentCenterChunk);
    }

    private Vector2Int CalculateCurrentPlayerChunkPos()
    {
        int gridX = Mathf.FloorToInt((Player.Instance.PlayerTransform.position.x + 
                                      halfCellSize) / chunkSize);
        int gridZ = Mathf.FloorToInt((Player.Instance.PlayerTransform.position.z + 
                                      halfCellSize) / chunkSize);

        return new Vector2Int(gridX, gridZ);
    }

    /// <summary>
    /// 주어진 중심 좌표를 기준으로 인접한 청크(예: 3x3)를 로드합니다.
    /// </summary>
    public void LoadChunksAround(Vector2Int center)
    {
        MapDataStorage MapDataStorage = DataManager.Instance.MapDataStorage;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkPos = new Vector2Int(center.x + x, center.y + y);
                if (!activeChunks.ContainsKey(chunkPos))
                {
                    // 청크 위치 계산 (청크 크기를 고려)
                    Vector3 spawnPosition = new Vector3(chunkPos.x * MapDataStorage.chunkSize, 0, chunkPos.y * MapDataStorage.chunkSize);

                    // 청크의 경우, 좌표에 따라 정해진 청크를 배분받아야 하기 때문에, 풀링 방식을 이용하기가 곤란함.
                    GameObject chunkObj = Instantiate(MapDataStorage.GetRandomMapChunk(chunkPos).gameObject, spawnPosition, Quaternion.identity, MapModelParent);
                    MapChunk newChunk = chunkObj.GetComponent<MapChunk>();
                    newChunk.Initialize(chunkPos, MapDataStorage);
                    activeChunks.Add(chunkPos, newChunk);
                }
            }
        }
    }

    /// <summary>
    /// 중앙 청크 변경 시, 새로운 청크를 로드하고 멀리 있는 청크를 언로드합니다.
    /// </summary>
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

    /// <summary>
    /// 중앙으로부터 멀리 떨어진 청크를 언로드합니다.
    /// </summary>
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
