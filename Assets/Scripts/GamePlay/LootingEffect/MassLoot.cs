using UnityEngine;

public class MassLoot : ILootEffect
{
    private float bonus;
    private float OriginalMass;

    public MassLoot(LootingRankType lootingRankType)
    {
        OriginalMass = Player.Instance.InitialStat.CurrentMass;
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 3f;
                break;
            case LootingRankType.Rare:
                bonus = 5f;
                break;
            case LootingRankType.Epic:
                bonus = 10f;
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
        player.Stat.CurrentMass += OriginalMass * 0.01f * bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player Gets {bonus}% bonus mass" };
    }

    public string GetEffectName()
    {
        return "Increase Weight";
    }
}
