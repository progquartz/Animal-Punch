using UnityEngine;

public class CriticalDamageLoot : ILootEffect
{
    private float bonus;

    public CriticalDamageLoot(LootingRankType lootingRankType)
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
        return new[] { $"Critical Damage increases {bonus}%" };
    }

    public string GetEffectName()
    {
        throw new System.NotImplementedException();
    }
}
