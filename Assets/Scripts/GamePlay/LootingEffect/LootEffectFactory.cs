
public enum LootingTypeType
{
    AccelerationLoot,
    DashForceLoot,
    MassLoot,
    SizeLoot,
    BaseDamageLoot,
    DashCooltimeLoot,
    ExpRatioLoot,
    CriticalChanceLoot,
    CritiaclDamageLoot,
}

public static class LootEffectFactory
{
    public static ILootEffect CreateEffect(LootingTypeType type, LootingRankType rank)
    {
        return type switch
        {
            LootingTypeType.AccelerationLoot => new AccelerationLoot(rank),
            // 다음과 같이 다른 클래스들도 추가:
            // LootingTypeType.MassLoot => new MassLoot(rank),
            _ => null
        };
    }
}
