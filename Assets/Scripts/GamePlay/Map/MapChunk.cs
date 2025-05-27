using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public Vector2Int chunkCoordinate;

    private MapDataStorage dataStorage;

    [SerializeField] private Transform PrepperParentTransform;
    private List<MapGroupObject> spawnedMapGroupObjects = new List<MapGroupObject>();

    private void Awake()
    {
        // 테스트 목적으로 Awake에서 초기화를 막음 (실제 환경에선 제거 권장)
        // TestChunk();
    }

    public void Initialize(Vector2Int coordinate, MapDataStorage data)
    {
        chunkCoordinate = coordinate;
        dataStorage = data;

        foreach (Transform holder in PrepperParentTransform)
        {
            string key = holder.gameObject.name.Split(' ')[0];
            if (string.IsNullOrEmpty(key)) continue;

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
