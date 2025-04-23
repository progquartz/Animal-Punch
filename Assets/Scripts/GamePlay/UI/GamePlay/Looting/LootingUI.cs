using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    [SerializeField] private LootingCardUI[] lootingCardUIList;

    public override void Show()
    {
        base.Show();
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
            int randomInt = Random.Range(0, 100);

            var rank = DataManager.Instance.LootingStorage.GetRandomLootingRank();
            lootingCardUIList[i].SetupCard(this, chosenTypes[i], rank);
        }
    }

    public void CloseLooting()
    {
        GameManager.Instance.ResumeTime();
        Close();
    }
}

