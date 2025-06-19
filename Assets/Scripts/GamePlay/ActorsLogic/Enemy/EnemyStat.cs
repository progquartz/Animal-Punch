using UnityEngine;

[System.Serializable]
public class EnemyStat
{
    [Header("체력")]
    public bool IsDead = false;
    public float HP;
    public float MaxHP;
    public float OriginalMaxHP;

    [Header("패턴 유형")]
    // 패턴 클래스 제작.

    [Header("물리적 속성")]
    public float Speed;
    public float Mass;
    public float MassOnDead;
    public float Drag;
    public float DragOnDead;
    public float AngularDrag;
    public float AngularDragOnDead;


    /// <summary>
    /// 데미지를 계산. 죽을 경우, true를 리턴.
    /// </summary>
    public bool HandleDamage(float damage)
    {
        //Debug.Log($"{damage}만큼의 데미지를 {HP}에 가합니다.");
        if(IsDead) return false;
        // 안 죽었을 경우
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
        IsDead =target.IsDead;
        HP = target.HP;
        MaxHP = target.MaxHP;
        OriginalMaxHP = target.OriginalMaxHP;


        Speed = target.Speed;
        Mass = target.Mass;
        MassOnDead = target.MassOnDead;
        Drag = target.Drag;
        DragOnDead = target.DragOnDead;
        AngularDrag = target.AngularDrag;
        AngularDragOnDead = target.AngularDragOnDead;
    }

    public void InitializeGameTimeScale()
    {
        MaxHP = OriginalMaxHP * GameManager.Instance.EnemyHealthRatio.Ratio;
        HP = MaxHP;
    }

}
