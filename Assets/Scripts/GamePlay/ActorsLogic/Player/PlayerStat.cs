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
    public float DashForce;                        // 앞으로 튀어나갈 힘
    public float DashChargeRatio;                     
    public float DashChargeTime = 2.0f;
    public float DashChargeMinTime = 0.3f;

    [Header("피버")]
    public int FeverGaugeBonus = 0;
    public float FeverBonusTime = 0f;
    public float FeverBoostChargeTime = 0.25f;

    [Header("같은 방향으로 이동할 때의 가속도")]
    public float AdditionForceRatio;                // 추가 가속도 비중
    public float AdditionForceMax;                  // 최대 추가 가속도

    [Header("회전")]
    public float RotationSpeed = 2000f;             // 최대 회전 속도.

    [Header("데미지")]
    public float BaseDamage;               // 기본 데미지
    public float AdditionalDamage;         // 추가 데미지
    public float ImpulseDamage;            // 충격량 데미지
    public float TotalDamageBonus = 0f;         // 데미지 총합 보너스

    [SerializeField]
    private float ImpulseStandard;          // 기준 충격량 (1배)
    public float ImpulseDamageBonusRatio;       // 충격량 비례 추가 데미지

    [Header("치명타 관련")]
    public int CriticalChance;
    public int CriticalDamageBonus;

    [Header("콤보 데미지")]
    public int ComboDamage = 1;
    public int ComboDamageAdditional = 0;

    [Header("콤보 시간")]
    public float comboBonusTime = 0f;

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
        amount = (int)(amount * (1 + (AdditionalExpRatio * 0.01f)));
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
        Player.Instance.OnLevelUp?.Invoke();
        int expLeft = CurrentExp - LevelUpExpNeed;
        Level++;
        CurrentExp = expLeft;
    }

    public bool IsCritical()
    {
        return Random.Range(0,100) <= CriticalChance;
    }

    public float GetComboCountRatio()
    {
        return (ComboDamage + ComboDamageAdditional) * 0.01f;
    }

    public float CalculateDamage(float impulseMagnitude, bool IsCritical)
    {
        // 기본 데미지
        float baseDamage = BaseDamage + AdditionalDamage;

        // 충격량 데미지
        Debug.Log($"ImpulseMagniture = {impulseMagnitude}");
        float impulseDamage = ImpulseDamage * (impulseMagnitude / ImpulseStandard) * (1 + 0.01f * ImpulseDamageBonusRatio);
        Debug.Log($"impulseMag = {impulseMagnitude} / impulsestandard = {ImpulseStandard} / impulsebonusratio = {ImpulseDamageBonusRatio} /  ImpulseDamage = {impulseDamage}");
        // 콤보 곱하기
        float comboRatio = Player.Instance.comboHandler.comboDamageRatio;

        // 일반 데미지 = (기본 데미지 + 충격량 데미지 ) * 콤보 배율
        float normalDamage = (baseDamage + impulseDamage) * comboRatio;

        if (IsCritical)
        {
            normalDamage *= 2f + (0.01f * CriticalDamageBonus);
        }

        return normalDamage;
    }

    public void CopyData(PlayerStat stat)
    {
        MoveForce = stat.MoveForce;                         // 이동할 때 가해지는 힘
        CurrentMass = stat.CurrentMass;
        CurrentSize = stat.CurrentSize;
        MaximalSize = stat.MaximalSize;


        DashForce = stat.DashForce;                        // 앞으로 튀어나갈 힘
        DashChargeRatio = stat.DashChargeRatio;                     // Space 키 쿨타임
        DashChargeTime = stat.DashChargeTime;
        DashChargeMinTime = stat.DashChargeMinTime;

        FeverBoostChargeTime = stat.FeverBoostChargeTime;
        FeverGaugeBonus = stat.FeverGaugeBonus;
        FeverBonusTime = stat.FeverBonusTime;

        AdditionForceRatio = stat.AdditionForceRatio;                // 추가 가속도 비중
        AdditionForceMax = stat.AdditionForceMax;                  // 최대 추가 가속도

        
        RotationSpeed = stat.RotationSpeed;             // 최대 회전 속도.

        
        BaseDamage = stat.BaseDamage;               // 기본 데미지
        AdditionalDamage = stat.AdditionalDamage;         // 추가 데미지
        ImpulseDamage = stat.AdditionalDamage;
        TotalDamageBonus = stat.TotalDamageBonus;


        ImpulseStandard = stat.ImpulseStandard;          // 기준 충격량 (1배)
        ImpulseDamageBonusRatio = stat.ImpulseDamageBonusRatio;       // 충격량 비례 추가 데미지
        

        CriticalChance = stat.CriticalChance;
        CriticalDamageBonus = stat.CriticalDamageBonus;


        ComboDamage = stat.ComboDamage;
        ComboDamageAdditional = stat.ComboDamageAdditional;

        Level = stat.Level;
        LevelUpExpNeed = stat.LevelUpExpNeed; // 레벨 업에 필요한 경험치.
        CurrentExp = stat.CurrentExp;
        AdditionalExpRatio = stat.AdditionalExpRatio;

    }


}
