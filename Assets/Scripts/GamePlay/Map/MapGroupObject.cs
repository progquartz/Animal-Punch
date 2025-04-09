using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.WSA;

public class MapGroupObject : MonoBehaviour
{
    public string key;

    /// <summary>
    /// �׷� ������Ʈ�� �ʱ�ȭ (�ʿ��� ��� �߰� ����)
    /// </summary>
    public void Initialize()
    {
        // Initialize�� �ϸ鼭 ���ο� �ִ� ��� ������Ʈ �̸��� �´� ������Ʈ Ǯ���ؿͼ� ��ü.
        Transform myTransform = this.transform;
        foreach(Transform child in myTransform)
        {
            string key = child.name.Split(' ')[0];

            // DataStorage���� �׷� ������ ã��.
            GameObject gp = MapManager.Instance.MapDataStorage.GetRandomModelPrefab(key);
            if (gp != null)
            {
                GameObject model = Instantiate(gp.gameObject, child);
                // ������Ʈ Ǯ���� �ش� Ű�� ��ü ������Ʈ ��ȯ
                model.transform.localPosition = Vector3.zero;
                Debug.Log($"�缳�� �� localPosition: {model.transform.localPosition}");
            }
        }
    }    
}
