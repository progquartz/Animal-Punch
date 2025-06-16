using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LootingRankProbability
{
    public LootingRankType Type;
    public int Probability;
}

public class LootingDataStorage : DataStorage
{
    public List<int> levelExpList = new List<int>();
    public List<Sprite> LootingEffectImages;
    public List<Sprite> LootingCardBackgroundImages;
    public List<LootingRankProbability> LootingProbabilityList;


    public void CheckResources()
    {
        Debug.Log($"LootingDataStorage - \n levelExpList.Count = {levelExpList.Count} \n/ lootingEffectImages.Count = {LootingEffectImages.Count} \n / LootingCardBackgroundImages.Count = {LootingCardBackgroundImages.Count} \n / LootingProbabilityList.Count = {LootingProbabilityList.Count}");
    }

    // 자동보정시키기
    private void NormalizeProbabilities()
    {
        int total = LootingProbabilityList.Sum(x => x.Probability);
        if (total == 0)
        {
            Debug.LogWarning("LootingProbabilityList의 총합이 0입니다.");
            return;
        }

        for (int i = 0; i < LootingProbabilityList.Count; i++)
        {
            var p = LootingProbabilityList[i];
            float normalized = (float)p.Probability / total * 100f;
            LootingProbabilityList[i].Probability = Mathf.RoundToInt(normalized);
        }
    }

    // 확률 기반으로 등급 하나 뽑기
    public LootingRankType GetRandomLootingRank()
    {
        int total = LootingProbabilityList.Sum(x => x.Probability);
        int rand = Random.Range(0, total);
        int cumulative = 0;

        foreach (var entry in LootingProbabilityList)
        {
            cumulative += entry.Probability;
            if (rand < cumulative)
                return entry.Type;
        }

        return LootingRankType.Normal;
    }

}
