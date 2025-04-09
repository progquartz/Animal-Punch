using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.WSA;

public class MapGroupObject : MonoBehaviour
{
    public string key;

    /// <summary>
    /// 그룹 오브젝트의 초기화 (필요한 경우 추가 로직)
    /// </summary>
    public void Initialize()
    {
        // Initialize를 하면서 내부에 있는 모든 오브젝트 이름에 맞는 오브젝트 풀링해와서 교체.
        Transform myTransform = this.transform;
        foreach(Transform child in myTransform)
        {
            string key = child.name.Split(' ')[0];

            // DataStorage에서 그룹 프리팹 찾기.
            GameObject gp = MapManager.Instance.MapDataStorage.GetRandomModelPrefab(key);
            if (gp != null)
            {
                GameObject model = Instantiate(gp.gameObject, child);
                // 오브젝트 풀에서 해당 키의 단체 오브젝트 반환
                model.transform.localPosition = Vector3.zero;
                Debug.Log($"재설정 후 localPosition: {model.transform.localPosition}");
            }
        }
    }    
}
