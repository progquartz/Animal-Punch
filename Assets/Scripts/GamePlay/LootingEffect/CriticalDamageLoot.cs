using UnityEngine;

public class CriticalDamageLoot : ILootEffect
{
    private int bonus;

    public CriticalDamageLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 2;
                break;
            case LootingRankType.Rare:
                bonus = 4;
                break;
            case LootingRankType.Epic:
                bonus = 8;
                break;
            case LootingRankType.Unique:
                bonus = 15;
                break;
            case LootingRankType.Legendary:
                bonus = 30;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        //player.Stat.acceleration += accelerationBonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Critical Damage increases {bonus}%" };
    }

    public string GetEffectName()
    {
        throw new System.NotImplementedException();
    }
}
