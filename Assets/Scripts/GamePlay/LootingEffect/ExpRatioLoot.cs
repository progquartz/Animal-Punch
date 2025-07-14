using UnityEngine;

public class ExpRatioLoot : ILootEffect
{
    private int bonus;

    public ExpRatioLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 3;
                break;
            case LootingRankType.Rare:
                bonus = 5;
                break;
            case LootingRankType.Epic:
                bonus = 10;
                break;
            
            case LootingRankType.Unique:
                bonus = 15;
                break;
            case LootingRankType.Legendary:
                bonus = 20;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.AdditionalExpRatio += bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player gets {bonus}% more Exp" };
    }

    public string GetEffectName()
    {
        return "EXP Bonus";
    }
}
