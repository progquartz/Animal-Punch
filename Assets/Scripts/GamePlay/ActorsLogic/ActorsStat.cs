using UnityEngine;

[System.Serializable]
public class ActorsStat
{
    [Header("이름")]
    public string ActorName;
    public GameObject ModelData;

    [Header("체력")]
    public bool IsDead = false;
    public float HP;
    public float MaxHP;

    [Header("패턴 유형")]
    // 패턴 클래스 제작.

    [Header("물리적 속성")]
    public float Speed;
    public float Mass;
    public float Drag;
    public float AngularDrag;


    /// <summary>
    /// 데미지를 계산. 죽을 경우, true를 리턴.
    /// </summary>
    public bool HandleDamage(float damage)
    {
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

}
