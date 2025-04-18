public class AccelerationLoot : ILootEffect
{
    private float accelerationBonus;

    public AccelerationLoot(LootingRankType rank)
    {
        switch (rank)
        {
            case LootingRankType.Normal: accelerationBonus = 1f; break;
            case LootingRankType.Rare: accelerationBonus = 2f; break;
            case LootingRankType.Unique: accelerationBonus = 3f; break;
            case LootingRankType.Epic: accelerationBonus = 4f; break;
            case LootingRankType.Legendary: accelerationBonus = 5f; break;
        }
    }

    public void ApplyEffect(Player player)
    {
        //player.Stat.acceleration += accelerationBonus;
    }

    public string[] GetEffectDescriptions()
    {
        return new string[] { $"플레이어의 가속도가 {accelerationBonus}만큼 증가합니다." };
    }

    public string GetEffectName()
    {
        throw new System.NotImplementedException();
    }
}
