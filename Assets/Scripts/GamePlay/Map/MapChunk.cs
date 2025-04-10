using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class MapChunk : MonoBehaviour
{
    public Vector2Int chunkCoordinate;

    private MapDataStorage dataStorage;

    [SerializeField] private Transform PrepperParentTransform;
    private List<MapGroupObject> spawnedMapGroupObjects = new List<MapGroupObject>();

    private void Awake()
    {
        TestChunk();
    }

    private void TestChunk()
    {
        Initialize(Vector2Int.zero,  MapManager.Instance.MapDataStorage);
    }
    /// <summary>
    /// ûũ�� �ʱ�ȭ �մϴ�.
    /// </summary>
    public void Initialize(Vector2Int coordinate, MapDataStorage data)
    {
        chunkCoordinate = coordinate;
        dataStorage = data;

        // �ڽ����� �ִ� Holder�� �̸��� ������� Transform�� �°� �ش� MapGroupObject�� ����.
        foreach (Transform holder in PrepperParentTransform)
        {
            string key = holder.gameObject.name.Split(' ')[0];
            if (string.IsNullOrEmpty(key)) continue;

            MapGroupObject gp = data.GetRandomGroupPrefab(key);
            if(gp != null)
            {
                GameObject groupObj = Instantiate(gp.gameObject);
                groupObj.transform.SetParent(holder, false);
                groupObj.transform.localPosition = Vector3.zero;

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
        }
        spawnedMapGroupObjects.Clear();

        // ûũ ��ü�� �ʿ� ���ٸ� ����
        Destroy(gameObject);
    }
}
