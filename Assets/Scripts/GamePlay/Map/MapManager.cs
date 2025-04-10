using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    public MapDataStorage MapDataStorage;
    public Transform MapModelParent;

    // 현재 활성화된 청크들을 좌표를 키로 저장
    private Dictionary<Vector2Int, MapChunk> activeChunks = new Dictionary<Vector2Int, MapChunk>();

    // 현재 중앙 청크 좌표 (ex. 플레이어 기준)
    public Vector2Int currentCenterChunk;
    private bool IsFirstTimeInitializing = true;
    private const float chunkSize = 90f;
    private const float halfCellSize = chunkSize * 0.5f; // 45

    void Start()
    {
        InitializeNearbyBlock();
        //LoadChunksAround(currentCenterChunk);
    }

    private void Update()
    {
        Vector2Int playerChunkPos = CalculateCurrentPlayerChunkPos();
        UpdateCenterChunk(playerChunkPos);
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

        Logger.Log($"현재 플레이어의 위치 : {Player.Instance.PlayerTransform.position}");
        Logger.Log($"현재 좌표 : {new Vector2Int(gridX, gridZ)}");

        return new Vector2Int(gridX, gridZ);
    }

    /// <summary>
    /// 주어진 중심 좌표를 기준으로 인접한 청크(예: 3x3)를 로드합니다.
    /// </summary>
    public void LoadChunksAround(Vector2Int center)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkPos = new Vector2Int(center.x + x, center.y + y);
                if (!activeChunks.ContainsKey(chunkPos))
                {
                    // 청크 위치 계산 (청크 크기를 고려)
                    Vector3 spawnPosition = new Vector3(chunkPos.x * MapDataStorage.chunkSize, 0, chunkPos.y * MapDataStorage.chunkSize);
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
