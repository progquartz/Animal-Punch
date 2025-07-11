using UnityEngine;

public class FeverTimeLoot : ILootEffect
{
    private float bonus;
    private float originalFeverTime;

    public FeverTimeLoot(LootingRankType lootingRankType)
    {
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 5f;
                break;
            case LootingRankType.Rare:
                bonus = 10f;
                break;
            case LootingRankType.Epic:
                bonus = 15f;
                break;
            case LootingRankType.Unique:
                bonus = 20f;
                break;
            case LootingRankType.Legendary:
                bonus = 30f;
                break;
        }
        originalFeverTime = Player.Instance.feverHandler.originalFeverTime;
    }

    public void ApplyEffect(Player player)
    {
        player.feverHandler.bonusTimeDuration += originalFeverTime * bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player FeverTime Gets {bonus}% more time" };
    }

    public string GetEffectName()
    {
        return "Increase FeverTime";
    }
}
