using UnityEngine;

[System.Serializable]
public class EnemyStat
{
    [Header("�̸�")]
    public string ActorName;

    [Header("ü�� ���� ����")]
    public bool IsEnemyHasHealth;
    [Header("���� ���� ����")]
    public bool IsEnemyHasCondition;

    [Header("���� ����")]
    public ActorBehaviourType BehaviourType;

    [Header("�� ������")]
    public GameObject ModelData;

    [Header("ü��")]
    public bool IsDead = false;
    public float HP;
    public float MaxHP;

    [Header("���� ����")]
    // ���� Ŭ���� ����.

    [Header("������ �Ӽ�")]
    public float Speed;
    public float Mass;
    public float MassOnDead;
    public float Drag;
    public float DragOnDead;
    public float AngularDrag;
    public float AngularDragOnDead;


    /// <summary>
    /// �������� ���. ���� ���, true�� ����.
    /// </summary>
    public bool HandleDamage(float damage)
    {
        Debug.Log($"{damage}��ŭ�� �������� {HP}�� ���մϴ�.");
        if(IsDead) return false;
        // �� �׾��� ���
        if(HP - damage > 0)
        {
            HP -= damage;
            return false;
        }
        else
        {
            HP = 0;
            IsDead = true;
            return true;
        }
    }

    public void CopyData(EnemyStat target)
    {
        ActorName = target.ActorName;
        ModelData = target.ModelData;

        IsEnemyHasHealth = target.IsEnemyHasHealth;        
        IsEnemyHasCondition = target.IsEnemyHasCondition;

        BehaviourType = target.BehaviourType;

        IsDead =target.IsDead;
        HP = target.HP;
        MaxHP = target.MaxHP;


        Speed = target.Speed;
        Mass = target.Mass;
        MassOnDead = target.MassOnDead;
        Drag = target.Drag;
        DragOnDead = target.DragOnDead;
        AngularDrag = target.AngularDrag;
        AngularDragOnDead = target.AngularDragOnDead;
    }

}
