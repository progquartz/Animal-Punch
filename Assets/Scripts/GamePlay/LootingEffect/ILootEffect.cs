public interface ILootEffect
{
    void ApplyEffect(Player player); // 실제 효과 적용
    string[] GetEffectDescriptions(); // UI용 설명 텍스트

    string GetEffectName();
}
