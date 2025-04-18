using System.Collections.Generic;
using UnityEngine;

public enum LootingRankType
{
    Normal = 0,
    Rare = 1,
    Unique = 2,
    Epic = 3,
    Legendary = 4,
}



public class LootingUI : BaseUI
{
    [SerializeField] public Color[] RankColors;
    [SerializeField] private LootingCardUI[] lootingCardUIList;

    void Start()
    {
        Show();
        ShowRandomLoots();
    }

    private void ShowRandomLoots()
    {
        var allTypes = System.Enum.GetValues(typeof(LootingTypeType));
        List<LootingTypeType> chosenTypes = new();

        while (chosenTypes.Count < 3)
        {
            var randomType = (LootingTypeType)allTypes.GetValue(Random.Range(0, allTypes.Length));
            if (!chosenTypes.Contains(randomType))
                chosenTypes.Add(randomType);
        }

        for (int i = 0; i < 3; i++)
        {
            var rank = (LootingRankType)Random.Range(0, RankColors.Length);
            lootingCardUIList[i].SetupCard(chosenTypes[i], rank, RankColors[(int)rank]);
        }
    }
}

