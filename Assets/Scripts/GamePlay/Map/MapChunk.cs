using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapChunk : MonoBehaviour
{
    // 이 청크의 좌표 (ex. 그리드상의 위치)
    public Vector2Int chunkCoordinate;

    private MapDataStorage dataStorage;
    private MapObjectPool objectPool;

    [SerializeField] private Transform PrepperParentTransform;
    // 해당 청크에서 실제로 생성된 그룹 오브젝트들을 추적
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
    /// 청크를 초기화 합니다.
    /// </summary>
    public void Initialize(Vector2Int coordinate, MapDataStorage data, MapObjectPool pool)
    {
        chunkCoordinate = coordinate;
        dataStorage = data;
        objectPool = pool;

        // 자식으로 있는 Holder의 이름을 기반으로 Transform에 맞게 해당 MapGroupObject를 생성.
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
                Debug.Log("여기서 제로로 만듬.");

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
            // 그룹 오브젝트 내부의 프리팹도 모두 반환
            groupObj.ReturnAllPrefabsToPool(objectPool);
            // 그룹 오브젝트 자체도 풀에 반환
            objectPool.ReturnToPool(groupObj.poolKey, groupObj.gameObject);
        }
        spawnedMapGroupObjects.Clear();

        // 청크 자체는 필요 없다면 삭제
        Destroy(gameObject);
    }
}
