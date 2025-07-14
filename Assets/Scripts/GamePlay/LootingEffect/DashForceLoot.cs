using UnityEngine;

public class DashForceLoot : ILootEffect
{
    private float bonus;

    public DashForceLoot(LootingRankType lootingRankType)
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
                bonus = 9f;
                break;
            case LootingRankType.Unique:
                bonus = 15f;
                break;
            case LootingRankType.Legendary:
                bonus = 30f;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.InitialStat.DashForce += Player.Instance.InitialStat.DashForce * 0.01f * bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Dash Force Gets {bonus}% Stronger" };
    }

    public string GetEffectName()
    {
        return "Dash Force";
    }
}
