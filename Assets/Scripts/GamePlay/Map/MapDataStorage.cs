using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class MapDataStorage : MonoBehaviour
{
    // 프로젝트 내에서 소환 가능한 모든 일반 프리팹
    public List<GameObject> ModelPrefabs = new List<GameObject>();
    // 프로젝트 내에서 소환 가능한 모든 그룹 프리팹 목록
    public List<MapGroupObject> groupPrefabs = new List<MapGroupObject>();
    // 프로젝트에서 소환 가능한 맵 청크 목록
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
            Debug.LogError("리스트가 null이거나 비어 있습니다.");
            return null;
        }

        // 간단한 해시 계산: 각 좌표에 임의의 소수를 곱해서 합한 후,
        // 음수가 나오지 않도록 절댓값을 취하고 리스트의 크기로 나눕니다.
        int hash = (pos.x * 17) + (pos.y * 31);
        hash = Mathf.Abs(hash);
        int index = hash % mapChunks.Count;
        return mapChunks[index];
    }

    // 맵 청크용 프리팹 (청크 내부에 단체 오브젝트를 배치할 위치 정보가 들어있음)
    public GameObject chunkPrefab;

    // 한 청크의 크기 (예: 20 단위)
    public int chunkSize = 20;
}
