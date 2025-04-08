using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class MapGroupObject : MonoBehaviour
{
    // 이 그룹의 식별 키 (ex. "Rock"). Inspector나 초기화 시 설정.
    public string poolKey;

    /// <summary>
    /// 그룹 오브젝트의 초기화 (필요한 경우 추가 로직)
    /// </summary>
    public void Initialize()
    {
        MapObjectPool objectPool = MapManager.Instance.MapObjectPool;
        // Initialize를 하면서 내부에 있는 모든 오브젝트 이름에 맞는 오브젝트 풀링해와서 교체.
        foreach(Transform poolingTransform in transform)
        {
            string key = poolingTransform.name.Split(' ')[0];

            // DataStorage에서 그룹 프리팹 찾기.
            GameObject gp = MapManager.Instance.MapDataStorage.ModelPrefabs.Find(prefab => prefab.name == key);

            if (gp != null)
            {
                // 오브젝트 풀에서 해당 키의 단체 오브젝트 반환
                GameObject model = objectPool.GetFromPool(key, gp);
                Logger.Log($"생성한 모델의 이름 = {model.name}");
                model.transform.SetParent(poolingTransform, true);
                Debug.Log($"부모로 만들자마자 localPosition: {model.transform.localPosition}");
                model.transform.localPosition = Vector3.zero;
                Debug.Log($"재설정 후 localPosition: {model.transform.localPosition}");
            }
        }


    }

    private void Update()
    {
        foreach (Transform poolingTransform in transform)
        {
            Debug.Log(poolingTransform.GetChild(0).gameObject.name + " " + poolingTransform.GetChild(0).transform.localPosition);
        }
    }

    /// <summary>
    /// 이 그룹 오브젝트 내부의 모든 프리팹(자식 객체)을 풀에 반환합니다.
    /// </summary>
    public void ReturnAllPrefabsToPool(MapObjectPool pool)
    {
        // 그룹 내에 여러 개의 개별 프리팹이 있을 경우 모두 반환
        // (단, 여기서는 단순 예제로 자식 오브젝트의 이름을 key로 사용)
        foreach (Transform child in transform)
        {
            string key = child.gameObject.name;
            pool.ReturnToPool(key, child.gameObject);
        }
    }
}
