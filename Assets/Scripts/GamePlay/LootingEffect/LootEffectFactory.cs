
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LootingTypeType
{
    AccelerationLoot = 0,
    DashForceLoot = 1,
    MassLoot = 2,
    SizeLoot = 3,
    DashCooltimeLoot = 4,
    ExpRatioLoot = 5,
    CriticalChanceLoot = 6,
    CritiaclDamageLoot = 7,
}

public static class LootEffectFactory
{
    public static ILootEffect CreateEffect(LootingTypeType type, LootingRankType rank)
    {
        return type switch
        {
            LootingTypeType.AccelerationLoot => new AccelerationLoot(rank),
            LootingTypeType.DashForceLoot => new DashForceLoot(rank),
            LootingTypeType.MassLoot => new MassLoot(rank),
            LootingTypeType.SizeLoot => (Player.Instance.Stat.CurrentSize <= Player.Instance.Stat.MaximalSize) ? new SizeLoot(rank) : new MassLoot(rank),
            LootingTypeType.DashCooltimeLoot => (Player.Instance.Stat.BoostCooltime >= Player.Instance.Stat.BoostMinimalCooltime) ? new DashCooltimeLoot(rank) : new DashForceLoot(rank),
            LootingTypeType.ExpRatioLoot =>  new ExpRatioLoot(rank),
            LootingTypeType.CriticalChanceLoot => (Player.Instance.Stat.CriticalChance <= 100) ? new CriticalChanceLoot(rank) : new CriticalDamageLoot(rank),
            LootingTypeType.CritiaclDamageLoot => new CriticalDamageLoot(rank),
            _ => null
        };
    }


    public static Sprite GetLootTypeImage(LootingTypeType type)
    {
        return DataManager.Instance.LootingStorage.LootingEffectImages[(int)type];
    }

    public static Sprite GetLootBackgroundImage(LootingRankType rank)
    {
        return DataManager.Instance.LootingStorage.LootingCardBackgroundImages[(int)rank];
    }
}
