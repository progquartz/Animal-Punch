using UnityEngine;

public class TotalDamageLoot : ILootEffect
{
    private float bonus;

    public TotalDamageLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 1f;
                break;
            case LootingRankType.Rare:
                bonus = 2f;
                break;
            case LootingRankType.Epic:
                bonus = 3f;
                break;
            case LootingRankType.Unique:
                bonus = 5f;
                break;
            case LootingRankType.Legendary:
                bonus = 10f;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.TotalDamageBonus += bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player All Damage Increases {bonus}%" };
    }

    public string GetEffectName()
    {
        return "All Damage";
    }
}
