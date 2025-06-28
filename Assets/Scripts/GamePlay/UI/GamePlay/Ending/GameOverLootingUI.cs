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
            Debug.Log($"{data.rankType.ToString()}랭크를 {data.count} 개 드랍합니다.");
            GameOverLootingUISlot slot = Instantiate(lootingslot, gridTransform).GetComponent<GameOverLootingUISlot>();

            LootingRankDesignTemplate designTemplate = rankDesignTemplateList.FirstOrDefault(o => o.rank == data.rankType);

            slot.SetUI(designTemplate, data.count);

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
