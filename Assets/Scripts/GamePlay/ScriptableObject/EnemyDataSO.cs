using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Object/Enemy/EnemyDataSO", order = int.MaxValue)]
public class EnemyDataSO : ScriptableObject
{
    public ActorsStat ActorsStat;
    // public AnimalAnimationController AnimationController; // 넣어야 할까?

}
