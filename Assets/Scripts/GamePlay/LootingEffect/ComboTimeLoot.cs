using UnityEngine;

public class ComboTimeLoot : ILootEffect
{
    private float bonus;

    public ComboTimeLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 0.1f;
                break;
            case LootingRankType.Rare:
                bonus = 0.2f;
                break;
            case LootingRankType.Epic:
                bonus = 0.5f;
                break;
            case LootingRankType.Unique:
                bonus = 1f;
                break;
            case LootingRankType.Legendary:
                bonus = 2f;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.comboHandler.comboBonusTime += bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player ComboTime Gets{bonus} sec" };
    }

    public string GetEffectName()
    {
        return "Increase ComboTime";
    }
}
