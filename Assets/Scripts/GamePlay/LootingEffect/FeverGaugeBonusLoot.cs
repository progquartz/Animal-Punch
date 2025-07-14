using UnityEngine;

public class FeverGaugeBonusLoot : ILootEffect
{
    private int bonus;

    public FeverGaugeBonusLoot(LootingRankType lootingRankType)
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
                bonus = 8;
                break;
            case LootingRankType.Unique:
                bonus = 10;
                break;
            case LootingRankType.Legendary:
                bonus = 20;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.FeverGaugeBonus += bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Fever Gauge Charges {bonus}% faster" };
    }

    public string GetEffectName()
    {
        return "Fever Bonus";
    }
}
