using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public Vector2Int chunkCoordinate;

    private MapDataStorage dataStorage;

    [SerializeField] private Transform PrepperParentTransform;
    private List<MapGroupObject> spawnedMapGroupObjects = new List<MapGroupObject>();

    public void Initialize(Vector2Int coordinate, MapDataStorage data)
    {
        chunkCoordinate = coordinate;
        dataStorage = data;

        foreach (Transform holder in PrepperParentTransform)
        {
            string key = holder.gameObject.name.Split(' ')[0];
            if (string.IsNullOrEmpty(key))
            {
                Debug.Log($"mapchunk에서 initialize하는 도중, key값으로 {key}를 가진 오브젝트가 발견되지 않음");
                continue;
            }
                

            MapGroupObject gpPrefab = data.GetRandomGroupPrefab(key);
            if (gpPrefab != null)
            {
                GameObject groupObj = MapObjectPool.Instance.GetFromPool(gpPrefab.key, gpPrefab.gameObject, holder);
                groupObj.transform.localPosition = Vector3.zero;

                MapGroupObject groupComponent = groupObj.GetComponent<MapGroupObject>();
                if (groupComponent != null)
                {
                    spawnedMapGroupObjects.Add(groupComponent);
                    groupComponent.Initialize();
                }
            }
            else
            {
                Debug.Log("MapGroundObject를 받아오지 못함!");
            }
        }
    }

    public void UnloadChunk()
    {
        foreach (MapGroupObject groupObj in spawnedMapGroupObjects)
        {
            groupObj.ReturnAllChildrenToPool();
            MapObjectPool.Instance.ReturnToPool(groupObj.key, groupObj.gameObject);
        }
        spawnedMapGroupObjects.Clear();

        // MapChunk 자체도 풀에 반환
        Destroy(this.gameObject);
    }
}
