using UnityEngine;

public class BaseAttackDamageLoot : ILootEffect
{
    private float bonus;

    public BaseAttackDamageLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 3f;
                break;
            case LootingRankType.Rare:
                bonus = 5f;
                break;
            case LootingRankType.Epic:
                bonus = 8f;
                break;
            case LootingRankType.Unique:
                bonus = 10f;
                break;
            case LootingRankType.Legendary:
                bonus = 20f;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.BaseDamage += bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Damage Increases {bonus}" };
    }

    public string GetEffectName()
    {
        return "Attack Damage";
    }
}
