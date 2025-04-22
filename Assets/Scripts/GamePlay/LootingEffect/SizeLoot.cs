using UnityEngine;

public class SizeLoot : ILootEffect
{
    private float bonus;
    private float originalSize;
    public SizeLoot(LootingRankType lootingRankType)
    {
        originalSize = Player.Instance.InitialStat.CurrentSize;
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 5f;
                break;
            case LootingRankType.Rare:
                bonus = 8f;
                break;
            case LootingRankType.Epic:
                bonus = 15f;
                break;
            case LootingRankType.Unique:
                bonus = 25f;
                break;
            case LootingRankType.Legendary:
                bonus = 40f;
                break;
        }
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.CurrentSize += originalSize * 0.01f * bonus;
        if(player.Stat.CurrentSize >= player.Stat.MaximalSize)
        {
            player.Stat.CurrentSize = player.Stat.MaximalSize;
        }
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Player Gets {bonus}% more size" };
    }
    public string GetEffectName()
    {
        return "Size Up";
    }
}
