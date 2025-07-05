using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LootingTypeType
{
    Acceleration = 0,
    DashForce = 1,
    Mass = 2,
    Size = 3,
    DashCooltime = 4,
    ExpRatio = 5,
    CriticalChance = 6,
    CriticalDamage = 7,
}

public static class LootEffectFactory
{
    public static ILootEffect CreateEffect(LootingTypeType type, LootingRankType rank)
    {
        return type switch
        {
            LootingTypeType.Acceleration => new AccelerationLoot(rank),
            LootingTypeType.DashForce => new DashForceLoot(rank),
            LootingTypeType.Mass => new MassLoot(rank),
            LootingTypeType.Size => (Player.Instance.Stat.CurrentSize <= Player.Instance.Stat.MaximalSize) ? new SizeLoot(rank) : new MassLoot(rank),
            LootingTypeType.DashCooltime => (Player.Instance.Stat.BoostChargeTime >= Player.Instance.Stat.BoostChargeMinimalTime) ? new DashCooltimeLoot(rank) : new DashForceLoot(rank),
            LootingTypeType.ExpRatio =>  new ExpRatioLoot(rank),
            LootingTypeType.CriticalChance => (Player.Instance.Stat.CriticalChance <= 100) ? new CriticalChanceLoot(rank) : new CriticalDamageLoot(rank),
            LootingTypeType.CriticalDamage => new CriticalDamageLoot(rank),
            _ => null
        };
    }


    public static Sprite GetLootTypeImage(LootingTypeType type)
    {
        return DataManager.Instance.LootingStorage.LootingEffectImages[(int)type];
    }

}
