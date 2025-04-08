using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    // MapDataStorage와 MapObjectPool은 인스펙터에서 할당하거나, 싱글톤/서비스로 관리할 수 있음.
    public MapDataStorage MapDataStorage;
    public MapObjectPool MapObjectPool;

    // 현재 활성화된 청크들을 좌표를 키로 저장
    private Dictionary<Vector2Int, MapChunk> activeChunks = new Dictionary<Vector2Int, MapChunk>();

    // 현재 중앙 청크 좌표 (ex. 플레이어 기준)
    public Vector2Int currentCenterChunk;

    void Start()
    {
        // 초기 중앙 청크 설정 (예: (0,0))
        currentCenterChunk = new Vector2Int(0, 0);
        //LoadChunksAround(currentCenterChunk);
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
                    GameObject chunkObj = Instantiate(MapDataStorage.chunkPrefab, spawnPosition, Quaternion.identity);
                    MapChunk newChunk = chunkObj.GetComponent<MapChunk>();
                    newChunk.Initialize(chunkPos, MapDataStorage, MapObjectPool);
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
        if (newCenter != currentCenterChunk)
        {
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
            // 기준 거리를 넘어간 청크라면 언로드 (여기서는 임의의 거리 기준 사용)
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
