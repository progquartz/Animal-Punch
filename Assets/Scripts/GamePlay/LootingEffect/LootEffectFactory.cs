using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LootingTypeType
{
    AccelerationDamage = 0, // 추가
    Acceleration = 1,
    BaseAttackDamage = 2, // 추가
    ComboTime = 3,
    CriticalChance = 4,
    CriticalDamage = 5,
    DashCooltime = 6,    
    DashForce = 7,
    ExpRatio = 8, // 추가
    FeverGaugeBonus = 9, // 추가
    FeverTime = 10, // 추가
    Mass = 11,
    Size = 12,
    TotalDamage = 13 // 추가
}

public static class LootEffectFactory
{
    public static ILootEffect CreateEffect(LootingTypeType type, LootingRankType rank)
    {
        return type switch
        {
            LootingTypeType.AccelerationDamage => new AccelerationDamageLoot(rank),
            LootingTypeType.Acceleration => new AccelerationLoot(rank),
            LootingTypeType.BaseAttackDamage => new BaseAttackDamageLoot(rank),
            LootingTypeType.ComboTime => new ComboTimeLoot(rank),
            LootingTypeType.CriticalChance => (Player.Instance.Stat.CriticalChance <= 100) ? new CriticalChanceLoot(rank) : new CriticalDamageLoot(rank),
            LootingTypeType.CriticalDamage => new CriticalDamageLoot(rank),
            LootingTypeType.DashCooltime => (Player.Instance.Stat.DashChargeTime >= Player.Instance.Stat.DashChargeMinTime) ? new DashCooltimeLoot(rank) : new DashForceLoot(rank),
            LootingTypeType.DashForce => new DashForceLoot(rank),
            LootingTypeType.ExpRatio => new ExpRatioLoot(rank),
            LootingTypeType.Mass => new MassLoot(rank),
            LootingTypeType.Size => (Player.Instance.Stat.CurrentSize <= Player.Instance.Stat.MaximalSize) ? new SizeLoot(rank) : new MassLoot(rank),
            LootingTypeType.TotalDamage => new TotalDamageLoot(rank),
            _ => null
        };
    }


    public static Sprite GetLootTypeImage(LootingTypeType type)
    {
        return DataManager.Instance.LootingStorage.LootingEffectImages[(int)type];
    }

}
