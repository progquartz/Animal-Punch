using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Object/Enemy/EnemyDataSO", order = int.MaxValue)]
public class EnemyDataSO : ScriptableObject
{
    public ActorsStat ActorsStat;
    public GameObject ActorPrefab;
    // public AnimalAnimationController AnimationController; // �־�� �ұ�?

}
