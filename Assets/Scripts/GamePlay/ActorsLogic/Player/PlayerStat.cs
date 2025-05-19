using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    [Header("현재 상태")]
    public float RigidbodySpeed = 0f;               // 속도
    public float CurrentAdditionForce = 0f;         // 현재 가속도
    public float LastDashTime;                      // 남은 대쉬 쿨타임

    [Header("크기 및 질량")]
    public float CurrentMass = 40f;
    public float CurrentSize = 1f;
    public float MaximalSize = 3f;

    [Header("이동 기본 추력")]
    public float MoveForce;                         // 이동할 때 가해지는 힘
    

    [Header("부스터")]
    public float BoostForce;                        // 앞으로 튀어나갈 힘
    public float BoostChargeRatio;                     // Space 키 쿨타임
    public float BoostChargeTime = 2.0f;
    public float BoostChargeMinimalTime = 0.3f;
    

    [Header("가속도")]
    public float AdditionForceRatio;                // 추가 가속도 비중
    public float AdditionForceMax;                  // 최대 추가 가속도

    [Header("회전")]
    public float RotationSpeed = 2000f;             // 최대 회전 속도.

    [Header("충격 데미지")]
    public float CollisionDamageBase;               // 기본 데미지
    public float CollisionDamageAdditional;         // 추가 데미지
    public float CollisionImpulseDamageBase;
    public float CollisionImpulseStandard;          // 기준 충격량 (1배)
    public float CollisionImpulseDamageRatio;       // 충격량 비례 추가 데미지

    [Header("치명타 관련")]
    public int CriticalChance;
    public int CriticalBonusDamage;

    [Header("경험치 및 레벨")]
    public int Level;
    public int LevelUpExpNeed; // 레벨 업에 필요한 경험치.
    public int CurrentExp;
    public int AdditionalExpRatio;


    [Header("애니메이션 관련 속성")]
    public float StandardAnimationSpeed = 6f;

    public void Init()
    {
        Level = 0;
        LevelUpExpNeed = DataManager.Instance.LootingStorage.levelExpList[0];
        CurrentExp = 0;
    }

    /// <summary>
    /// 만약 레벨업 한다면 true 리턴
    /// </summary>
    public bool GainExp(int amount)
    {
        CurrentExp += amount;

        if (CurrentExp >= LevelUpExpNeed)
        {
            OnLevelUp();
            return true;
        }
        return false;
    }

    public void OnLevelUp()
    {
        int expLeft = CurrentExp - LevelUpExpNeed;
        Level++;
        CurrentExp = expLeft;
    }

    public bool IsCritical()
    {
        return Random.Range(0,100) <= CriticalChance;
    }

    public float CalculateDamage(float impulseMagnitude, bool IsCritical)
    {

        float baseDamage = CollisionDamageBase + CollisionDamageAdditional;
        float impulseDamage = CollisionImpulseDamageBase * (impulseMagnitude / CollisionImpulseStandard) * CollisionImpulseDamageRatio;

        float totalDamage = baseDamage + impulseDamage;

        if (IsCritical)
        {
            totalDamage *= 2f + (0.01f * CriticalBonusDamage);
        }

        return totalDamage;
    }

    public void CopyData(PlayerStat stat)
    {
        MoveForce = stat.MoveForce;                         // 이동할 때 가해지는 힘



        BoostForce = stat.BoostForce;                        // 앞으로 튀어나갈 힘
        BoostChargeRatio = stat.BoostChargeRatio;                     // Space 키 쿨타임
        BoostChargeTime = stat.BoostChargeTime;
        BoostChargeMinimalTime = stat.BoostChargeMinimalTime;


        AdditionForceRatio = stat.AdditionForceRatio;                // 추가 가속도 비중
        AdditionForceMax = stat.AdditionForceMax;                  // 최대 추가 가속도

        
        RotationSpeed = stat.RotationSpeed;             // 최대 회전 속도.

        
        CollisionDamageBase = stat.CollisionDamageBase;               // 기본 데미지
        CollisionDamageAdditional = stat.CollisionDamageAdditional;         // 추가 데미지
        CollisionImpulseDamageBase = stat.CollisionDamageAdditional;
        CollisionImpulseStandard = stat.CollisionImpulseStandard;          // 기준 충격량 (1배)
        CollisionImpulseDamageRatio = stat.CollisionImpulseDamageRatio;       // 충격량 비례 추가 데미지

        
        CriticalChance = stat.CriticalChance;
        CriticalBonusDamage = stat.CriticalBonusDamage;

        
        Level = stat.Level;
        LevelUpExpNeed = stat.LevelUpExpNeed; // 레벨 업에 필요한 경험치.
        CurrentExp = stat.CurrentExp;
        AdditionalExpRatio = stat.AdditionalExpRatio;
}


}
