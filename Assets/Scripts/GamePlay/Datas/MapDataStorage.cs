using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class MapDataStorage : MonoBehaviour
{
    // 프로젝트 내에서 소환 가능한 모든 일반 프리팹
    public List<GameObject> ModelPrefabs = new List<GameObject>();
    // 프로젝트 내에서 소환 가능한 모든 그룹 프리팹 
    public List<MapGroupObject> groupPrefabs = new List<MapGroupObject>();
    // 프로젝트에서 소환 가능한 맵 청크 목록
    public List<MapChunk> mapChunks = new List<MapChunk>();
    public int chunkSize = 90;

    public GameObject GetRandomModelPrefab(string key)
    {
        List<GameObject> list = ModelPrefabs.FindAll(prefab => prefab.name == key);
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    public MapGroupObject GetRandomGroupPrefab (string key)
    {
        List<MapGroupObject> list = groupPrefabs.FindAll(x => x.key == key);
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

        // 결과값 고정을 위한 해싱
        int hash = (pos.x * 17) + (pos.y * 31);
        hash = Mathf.Abs(hash);
        int index = hash % mapChunks.Count;
        return mapChunks[index];
    }


}
