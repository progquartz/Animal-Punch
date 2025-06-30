using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct LootingData
{
    public BoxRankType rankType;
    public int count;

    public LootingData(BoxRankType rank, int count)
    {
        this.rankType = rank;
        this.count = count;
    }
}

public class BoxInventoryManager : SingletonBehaviour<BoxInventoryManager>
{
    public BoxInventory inventory = new();
    public int maxDropScore = 100;

    private string savePath => Path.Combine(Application.persistentDataPath, "boxinventory.json");

    protected override void Init()
    {
        base.Init();
        Load();
    }

    public void AddBox(BoxRankType boxId)
    {
        inventory.Add(boxId);
        Save();
    }

    public bool AssignBox(int inventoryIndex, int slotIndex)
    {
        BoxRankType keyTryOpening = inventory.GetType(inventoryIndex);
        bool result = inventory.Use(inventoryIndex);
        if(result)
        {
            BoxDataSO boxData = DataManager.Instance.LootingStorage.AllBoxDataList.Find(b => b.rank == keyTryOpening);
            // 슬롯 지정 없이 호출되었을 경우.
            if (slotIndex == -1)
            {
                slotIndex = BoxSlotManager.Instance.GetSlotEmpty();
            }
            BoxSlotManager.Instance.AssignBoxToSlot(slotIndex, boxData);
        }
        
        if (result) Save();
        return result;
    }

    public List<BoxInventory.BoxCount> GetOwnedBoxes()
    {
        return inventory.GetAllOwnedBoxes();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            inventory = JsonUtility.FromJson<BoxInventory>(json);
        }
        else
        {
            inventory = new BoxInventory();
            Save();
        }
    }

    public List<LootingData> CalculateGameEndingLoots(int totalScore)
    {
        var result = new List<LootingData>();
        int usedDropScore = 0;
        var loots = DataManager.Instance.LootingStorage.AllBoxDataList
            .OrderByDescending(item => item.dropScore)
            .ToList();
        loots.Add(DataManager.Instance.LootingStorage.GemData);
        loots.Add(DataManager.Instance.LootingStorage.GoldData);

        foreach (var item in loots)
        {
            Debug.Log($"[LootSystem] → Checking item: {item.name}, Rank: {item.rank}, MinScore: {item.minScore}");

            if (totalScore < item.minScore)
            {
                continue;
            }

            int bonusCount = Mathf.Max(0, (totalScore - item.minScore) / item.bonusScoreStep);
            float totalChance = item.baseRate + (bonusCount * item.bonusRate);

            int guaranteedCount = Mathf.FloorToInt(totalChance / 100f);
            float leftoverChance = totalChance % 100f;

            Debug.Log($"totalChance: {totalChance}");
            int finalCount = 0;

            // 확정 드랍
            for (int i = 0; i < guaranteedCount; i++)
            {
                if (usedDropScore + item.dropScore > maxDropScore)
                {
                    break;
                }

                usedDropScore += item.dropScore;
                finalCount++;
            }

            // 추가 확률 드랍
            if (leftoverChance > 0f &&
                usedDropScore + item.dropScore <= maxDropScore)
            {
                float roll = Random.Range(0f, 100f);

                if (roll < leftoverChance)
                {
                    usedDropScore += item.dropScore;
                    finalCount++;
                }
            }

            if (finalCount > 0)
            {
                int index = result.FindIndex(x => x.rankType == item.rank);
                if (index >= 0)
                {
                    result[index] = new LootingData(item.rank, result[index].count + finalCount);
                }
                else
                {
                    result.Add(new LootingData(item.rank, finalCount));
                }
            }
        }

        foreach (var data in result)
        {
            Debug.Log($"{data.rankType.ToString()}랭크를 {data.count} 개 드랍합니다.");
        }

        return result;
    }

    public void GetLoot(List<LootingData> lootingDatas)
    {
        foreach (var data in lootingDatas)
        {
            // 골드 처리
            if (data.rankType == BoxRankType.Gold)
            {

            }
            // 보석 처리.
            else if (data.rankType == BoxRankType.Gem)
            {

            }
            // 상자 처리.
            else
            {

            }
        }
    }

    public void TestLoot()
    {
        AddBox(BoxRankType.Normal);
    }
}
