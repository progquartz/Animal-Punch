using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapChunk : MonoBehaviour
{
    // �� ûũ�� ��ǥ (ex. �׸������ ��ġ)
    public Vector2Int chunkCoordinate;

    private MapDataStorage dataStorage;
    private MapObjectPool objectPool;

    [SerializeField] private Transform PrepperParentTransform;
    // �ش� ûũ���� ������ ������ �׷� ������Ʈ���� ����
    private List<MapGroupObject> spawnedMapGroupObjects = new List<MapGroupObject>();

    private void Start()
    {
        TestChunk();
    }
    private void TestChunk()
    {
        Initialize(Vector2Int.zero, MapManager.Instance.MapDataStorage, MapManager.Instance.MapObjectPool);
    }
    /// <summary>
    /// ûũ�� �ʱ�ȭ �մϴ�.
    /// </summary>
    public void Initialize(Vector2Int coordinate, MapDataStorage data, MapObjectPool pool)
    {
        chunkCoordinate = coordinate;
        dataStorage = data;
        objectPool = pool;

        // �ڽ����� �ִ� Holder�� �̸��� ������� Transform�� �°� �ش� MapGroupObject�� ����.
        foreach (Transform holder in PrepperParentTransform)
        {
            MapGroupObject mapGroupObject = holder.GetComponent<MapGroupObject>();
            if (mapGroupObject != null)
            {
                mapGroupObject.Initialize();
            }
            string key = holder.gameObject.name.Split('_')[0];
            if (string.IsNullOrEmpty(key)) continue;

            MapGroupObject gp = data.GetRandomGroupPrefab(key);
            if(gp != null)
            {
                GameObject groupObj = Instantiate(gp.gameObject);
                groupObj.transform.SetParent(holder, false);
                groupObj.transform.localPosition = Vector3.zero;
                Debug.Log("���⼭ ���η� ����.");

                // MapGroupObject ������Ʈ�� �޾� �߰� �ʱ�ȭ ����
                MapGroupObject groupComponent = groupObj.GetComponent<MapGroupObject>();
                if (groupComponent != null)
                {
                    spawnedMapGroupObjects.Add(groupComponent);
                    groupComponent.Initialize();
                }
            }
        }
    }

    /// <summary>
    /// ûũ ��ε� ��, ���� ��ü ������Ʈ�� ��� Ǯ�� ��ȯ�մϴ�.
    /// </summary>
    public void UnloadChunk()
    {
        foreach (MapGroupObject groupObj in spawnedMapGroupObjects)
        {
            // �׷� ������Ʈ ������ �����յ� ��� ��ȯ
            groupObj.ReturnAllPrefabsToPool(objectPool);
            // �׷� ������Ʈ ��ü�� Ǯ�� ��ȯ
            objectPool.ReturnToPool(groupObj.poolKey, groupObj.gameObject);
        }
        spawnedMapGroupObjects.Clear();

        // ûũ ��ü�� �ʿ� ���ٸ� ����
        Destroy(gameObject);
    }
}
