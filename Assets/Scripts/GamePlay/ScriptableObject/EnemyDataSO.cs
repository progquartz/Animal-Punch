using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Object/Enemy/EnemyDataSO", order = int.MaxValue)]
public class EnemyDataSO : ScriptableObject
{
    [Header("이름과 키")]
    public string ActorName;
    public string ActorKey;

    [Header("체력 보유 여부")]
    public bool IsEnemyHasHealth;
    [Header("상태 보유 여부")]
    public bool IsEnemyHasCondition;

    [Header("최대 스폰 가능 개수")]
    public int MaxSpawnCount;

    [Header("스폰 주기 및 개수")]
    public float SpawnInterval;
    public int SpawnCount;
    public bool NeedSpawnRightNow;


    [Header("최소 스폰 시간")]
    public float MinSpawnTime;
    [Header("최대 스폰 시간")]
    public float MaxSpawnTime;

    [Header("모델 오브젝트")]
    public GameObject ModelObject;


    [Header("패턴 유형")]
    public ActorBehaviourType BehaviourType;

    public EnemyStat ActorsStat;
    public DropItemData DropItemData;
    // public AnimalAnimationController AnimationController; // 넣어야 할까?

}
