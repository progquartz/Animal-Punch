using UnityEngine;

public class BaseAttackDamageLoot : ILootEffect
{
    private float bonus;
    private float BaseMoveForce = 3000f;

    public AccelerationLoot(LootingRankType lootingRankType)
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
        BaseMoveForce = Player.Instance.InitialStat.MoveForce;
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
