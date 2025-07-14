public class AccelerationLoot : ILootEffect
{
    private float bonus;

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
    }

    public void ApplyEffect(Player player)
    {
        player.Stat.MoveForce += player.InitialStat.MoveForce * 0.01f * bonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new[] { $"Acceleration Gets {bonus}% faster" };
    }

    public string GetEffectName()
    {
        return "Speed";
    }
}
