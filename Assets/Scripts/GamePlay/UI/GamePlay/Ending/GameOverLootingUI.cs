using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LootingRankDesignTemplate
{
    public BoxRankType rank;
    public Color backgroundColor;
    public Color foregroundColor;
    public Sprite sprite;
}

public class GameOverLootingUI : MonoBehaviour
{
    public Transform gridTransform;
    public LootingRankDesignTemplate[] rankDesignTemplateList;
    public GameObject lootingslot;

    public bool isActivated = false;

    public void ShowLoot(int totalScore, List<LootingData> lootingData)
    {
        Debug.Log("GameOverLootingUI Working");
        foreach (LootingData data in lootingData)
        {
            GameObject slot = Instantiate(lootingslot, gridTransform);
            LootingRankDesignTemplate designTemplate = rankDesignTemplateList.FirstOrDefault(o => o.rank == data.rankType);

            slot.GetComponent<GameOverLootingUISlot>().SetUI(designTemplate, data.count);

            if (data.rankType == BoxRankType.Gold)
            {

            }
            else if (data.rankType == BoxRankType.Gem)
            {

            }
            else
            {

            }

        }



        // LootingManager에서 모든 Looting 가져오도록 하기.
    }


}
