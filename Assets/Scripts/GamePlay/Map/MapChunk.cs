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
    /// 청크를 초기화 합니다.
    /// </summary>
    public void Initialize(Vector2Int coordinate, MapDataStorage data)
    {
        chunkCoordinate = coordinate;
        dataStorage = data;

        // 자식으로 있는 Holder의 이름을 기반으로 Transform에 맞게 해당 MapGroupObject를 생성.
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

                // MapGroupObject 컴포넌트를 받아 추가 초기화 진행
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
    /// 청크 언로드 시, 내부 단체 오브젝트를 모두 풀로 반환합니다.
    /// </summary>
    public void UnloadChunk()
    {
        foreach (MapGroupObject groupObj in spawnedMapGroupObjects)
        {
        }
        spawnedMapGroupObjects.Clear();

        // 청크 자체는 필요 없다면 삭제
        Destroy(gameObject);
    }
}
