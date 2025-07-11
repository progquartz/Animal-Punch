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
        player.Stat.MoveForce += BaseMoveForce * 0.01f * bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player Acceleration Gets {bonus}% faster" };
    }

    public string GetEffectName()
    {
        return "Increase Speed";
    }
}
