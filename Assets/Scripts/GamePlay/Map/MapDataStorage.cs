using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class MapDataStorage : MonoBehaviour
{
    // ������Ʈ ������ ��ȯ ������ ��� �Ϲ� ������
    public List<GameObject> ModelPrefabs = new List<GameObject>();
    // ������Ʈ ������ ��ȯ ������ ��� �׷� ������ ���
    public List<MapGroupObject> groupPrefabs = new List<MapGroupObject>();
    // ������Ʈ���� ��ȯ ������ �� ûũ ���
    public List<MapChunk> mapChunks = new List<MapChunk>();

    public MapGroupObject GetRandomGroupPrefab (string key)
    {
        List<MapGroupObject> list = groupPrefabs.FindAll(x => x.poolKey == key);
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    public MapChunk GetRandomMapChunk(Vector2Int pos)
    {
        if (mapChunks == null || mapChunks.Count == 0)
        {
            Debug.LogError("����Ʈ�� null�̰ų� ��� �ֽ��ϴ�.");
            return null;
        }

        // ������ �ؽ� ���: �� ��ǥ�� ������ �Ҽ��� ���ؼ� ���� ��,
        // ������ ������ �ʵ��� ������ ���ϰ� ����Ʈ�� ũ��� �����ϴ�.
        int hash = (pos.x * 17) + (pos.y * 31);
        hash = Mathf.Abs(hash);
        int index = hash % mapChunks.Count;
        return mapChunks[index];
    }

    // �� ûũ�� ������ (ûũ ���ο� ��ü ������Ʈ�� ��ġ�� ��ġ ������ �������)
    public GameObject chunkPrefab;

    // �� ûũ�� ũ�� (��: 20 ����)
    public int chunkSize = 20;
}
