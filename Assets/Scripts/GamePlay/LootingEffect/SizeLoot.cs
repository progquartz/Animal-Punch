using UnityEngine;

public class SizeLoot : ILootEffect
{
    private float bonus;

    public SizeLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                break;
            case LootingRankType.Rare:
                break;
            case LootingRankType.Epic:
                break;
            case LootingRankType.Legendary:
                break;
            case LootingRankType.Unique:
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        //player.Stat.acceleration += accelerationBonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player Gets {bonus}% more size" };
    }
    public string GetEffectName()
    {
        throw new System.NotImplementedException();
    }
}
