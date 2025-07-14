using UnityEngine;

public class FeverTimeLoot : ILootEffect
{
    private float bonus;

    public FeverTimeLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 0.25f;
                break;
            case LootingRankType.Rare:
                bonus = 0.5f;
                break;
            case LootingRankType.Epic:
                bonus = 1f;
                break;
            case LootingRankType.Unique:
                bonus = 2f;
                break;
            case LootingRankType.Legendary:
                bonus = 3f;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.FeverBonusTime += bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"{bonus}% more Fever Time" };
    }

    public string GetEffectName()
    {
        return "Fever Time";
    }
}
