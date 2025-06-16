using UnityEngine;
using System.Collections.Generic;

public class MapGroupObject : MonoBehaviour
{
    public string key;
    private List<GameObject> pooledObjects = new List<GameObject>();

    public void Initialize()
    {
        foreach (Transform childHolder in transform)
        {
            string childKey = childHolder.name.Split(' ')[0];
            if (string.IsNullOrEmpty(childKey)) continue;

            GameObject prefab = DataManager.Instance.MapDataStorage.GetRandomModelPrefab(childKey);
            if (prefab != null)
            {
                GameObject model = MapObjectPool.Instance.GetFromPool(childKey, prefab, childHolder);
                model.transform.localPosition = Vector3.zero;
                pooledObjects.Add(model);
            }
        }
    }

    // 자식 오브젝트 전부 풀로 반환
    public void ReturnAllChildrenToPool()
    {
        foreach (GameObject obj in pooledObjects)
        {
            string returnKey = obj.name.Split('(')[0].Trim(); // Instantiate 시 Unity가 추가한 (Clone) 제거
            MapObjectPool.Instance.ReturnToPool(returnKey, obj);
        }
        pooledObjects.Clear();
    }
}
