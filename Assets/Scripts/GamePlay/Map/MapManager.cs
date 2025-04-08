using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    // MapDataStorage�� MapObjectPool�� �ν����Ϳ��� �Ҵ��ϰų�, �̱���/���񽺷� ������ �� ����.
    public MapDataStorage MapDataStorage;
    public MapObjectPool MapObjectPool;

    // ���� Ȱ��ȭ�� ûũ���� ��ǥ�� Ű�� ����
    private Dictionary<Vector2Int, MapChunk> activeChunks = new Dictionary<Vector2Int, MapChunk>();

    // ���� �߾� ûũ ��ǥ (ex. �÷��̾� ����)
    public Vector2Int currentCenterChunk;

    void Start()
    {
        // �ʱ� �߾� ûũ ���� (��: (0,0))
        currentCenterChunk = new Vector2Int(0, 0);
        //LoadChunksAround(currentCenterChunk);
    }

    /// <summary>
    /// �־��� �߽� ��ǥ�� �������� ������ ûũ(��: 3x3)�� �ε��մϴ�.
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
                    // ûũ ��ġ ��� (ûũ ũ�⸦ ���)
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
    /// �߾� ûũ ���� ��, ���ο� ûũ�� �ε��ϰ� �ָ� �ִ� ûũ�� ��ε��մϴ�.
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
    /// �߾����κ��� �ָ� ������ ûũ�� ��ε��մϴ�.
    /// </summary>
    private void UnloadChunksNotNearCenter(Vector2Int center)
    {
        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in activeChunks)
        {
            // ���� �Ÿ��� �Ѿ ûũ��� ��ε� (���⼭�� ������ �Ÿ� ���� ���)
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
