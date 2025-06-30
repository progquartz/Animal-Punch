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
        for(int i = 0; i < lootingData.Count; i++)
        {
            GameObject slot = Instantiate(lootingslot, gridTransform);
            LootingRankDesignTemplate designTemplate = rankDesignTemplateList.FirstOrDefault(o => o.rank == lootingData[i].rankType);

            float slotWaitingTime = i * 0.1f;
            slot.GetComponent<GameOverLootingUISlot>().SetUI(designTemplate,lootingData[i].count);
        }



        // LootingManager에서 모든 Looting 가져오도록 하기.
    }


}
