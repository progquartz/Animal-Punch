using UnityEngine;

public class DashCooltimeLoot : ILootEffect
{

    private float bonus;
    private float originalBoostCooltime;

    public DashCooltimeLoot(LootingRankType lootingRankType)
    {
        originalBoostCooltime = Player.Instance.InitialStat.BoostCooltime;
        switch (lootingRankType)
        {
            case LootingRankType.Normal:
                bonus = 2f;
                break;
            case LootingRankType.Rare:
                bonus = 4f;
                break;
            case LootingRankType.Epic:
                bonus = 6f;
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
        player.Stat.BoostCooltime -= originalBoostCooltime * 0.01f * bonus;
        if(player.Stat.BoostCooltime <= player.Stat.BoostMinimalCooltime)
        {
            player.Stat.BoostCooltime = player.Stat.BoostMinimalCooltime;
        }
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Dash Cooltimes reduces {bonus}%" };
    }

    public string GetEffectName()
    {
        return "Dash Cooltime";
    }
}
