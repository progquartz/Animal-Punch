using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonBehaviour<MapManager>
{
    public MapDataStorage MapDataStorage;
    public Transform MapModelParent;

    // ���� Ȱ��ȭ�� ûũ���� ��ǥ�� Ű�� ����
    private Dictionary<Vector2Int, MapChunk> activeChunks = new Dictionary<Vector2Int, MapChunk>();

    // ���� �߾� ûũ ��ǥ (ex. �÷��̾� ����)
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

        Logger.Log($"���� �÷��̾��� ��ġ : {Player.Instance.PlayerTransform.position}");
        Logger.Log($"���� ��ǥ : {new Vector2Int(gridX, gridZ)}");

        return new Vector2Int(gridX, gridZ);
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
                    GameObject chunkObj = Instantiate(MapDataStorage.GetRandomMapChunk(chunkPos).gameObject, spawnPosition, Quaternion.identity, MapModelParent);
                    MapChunk newChunk = chunkObj.GetComponent<MapChunk>();
                    newChunk.Initialize(chunkPos, MapDataStorage);
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
        if (IsFirstTimeInitializing || newCenter != currentCenterChunk)
        {
            IsFirstTimeInitializing = false;
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
