using UnityEngine;

public class CriticalChanceLoot : ILootEffect
{
    private int bonus;

    public CriticalChanceLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 1;
                break;
            case LootingRankType.Rare:
                bonus = 2;
                break;
            case LootingRankType.Epic:
                bonus = 4;
                break;
            case LootingRankType.Unique:
                bonus = 8;
                break;
            case LootingRankType.Legendary:
                bonus = 15;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.CriticalChance += bonus;
        if(player.Stat.CriticalChance >= 100 )
        {
            player.Stat.CriticalChance = 100;
        }
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Critical Chance increases {bonus}%" };
    }

    public string GetEffectName()
    {
        return "Critical Chance";
    }
}
