using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class MapGroupObject : MonoBehaviour
{
    // �� �׷��� �ĺ� Ű (ex. "Rock"). Inspector�� �ʱ�ȭ �� ����.
    public string poolKey;

    /// <summary>
    /// �׷� ������Ʈ�� �ʱ�ȭ (�ʿ��� ��� �߰� ����)
    /// </summary>
    public void Initialize()
    {
        MapObjectPool objectPool = MapManager.Instance.MapObjectPool;
        // Initialize�� �ϸ鼭 ���ο� �ִ� ��� ������Ʈ �̸��� �´� ������Ʈ Ǯ���ؿͼ� ��ü.
        foreach(Transform poolingTransform in transform)
        {
            string key = poolingTransform.name.Split(' ')[0];

            // DataStorage���� �׷� ������ ã��.
            GameObject gp = MapManager.Instance.MapDataStorage.ModelPrefabs.Find(prefab => prefab.name == key);

            if (gp != null)
            {
                // ������Ʈ Ǯ���� �ش� Ű�� ��ü ������Ʈ ��ȯ
                GameObject model = objectPool.GetFromPool(key, gp);
                Logger.Log($"������ ���� �̸� = {model.name}");
                model.transform.SetParent(poolingTransform, true);
                Debug.Log($"�θ�� �����ڸ��� localPosition: {model.transform.localPosition}");
                model.transform.localPosition = Vector3.zero;
                Debug.Log($"�缳�� �� localPosition: {model.transform.localPosition}");
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
    /// �� �׷� ������Ʈ ������ ��� ������(�ڽ� ��ü)�� Ǯ�� ��ȯ�մϴ�.
    /// </summary>
    public void ReturnAllPrefabsToPool(MapObjectPool pool)
    {
        // �׷� ���� ���� ���� ���� �������� ���� ��� ��� ��ȯ
        // (��, ���⼭�� �ܼ� ������ �ڽ� ������Ʈ�� �̸��� key�� ���)
        foreach (Transform child in transform)
        {
            string key = child.gameObject.name;
            pool.ReturnToPool(key, child.gameObject);
        }
    }
}
