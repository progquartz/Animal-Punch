using UnityEngine;

public class DashCooltimeLoot : ILootEffect
{

    private float bonus;
    private string desc;

    public DashCooltimeLoot(LootingRankType lootingRankType)
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
        return new[] { $"Dash Cooltimes reduces {bonus}%" };
    }

    public string GetEffectName()
    {
        throw new System.NotImplementedException();
    }
}
