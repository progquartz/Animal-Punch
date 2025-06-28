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
            BoxDataSO boxData = BoxSlotManager.Instance.AllBoxDataList.Find(b => b.rank == keyTryOpening);
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

        Debug.Log($"[LootSystem] totalScore: {totalScore}, maxDropScore: {maxDropScore}");

        var lootItems = BoxSlotManager.Instance.AllBoxDataList
            .OrderByDescending(item => item.dropScore)
            .ToList();

        Debug.Log($"[LootSystem] Loaded {lootItems.Count} items from BoxSlotManager.");

        foreach (var item in lootItems)
        {
            Debug.Log($"[LootSystem] → Checking item: {item.name}, Rank: {item.rank}, MinScore: {item.minScore}");

            if (totalScore < item.minScore)
            {
                Debug.Log($"  Skipped: totalScore({totalScore}) < minScore({item.minScore})");
                continue;
            }

            int bonusCount = Mathf.Max(0, (totalScore - item.minScore) / item.bonusScoreStep);
            float totalChance = item.baseRate + (bonusCount * item.bonusRate);

            int guaranteedCount = Mathf.FloorToInt(totalChance / 100f);
            float leftoverChance = totalChance % 100f;

            Debug.Log($"  baseRate: {item.baseRate}, bonusRate: {item.bonusRate}, bonusStep: {item.bonusScoreStep}");
            Debug.Log($"  → totalChance: {totalChance}, guaranteedCount: {guaranteedCount}, leftoverChance: {leftoverChance}");

            int finalCount = 0;

            // 확정 드랍
            for (int i = 0; i < guaranteedCount; i++)
            {
                if (usedDropScore + item.dropScore > maxDropScore)
                {
                    Debug.Log($"  Stop guaranteed drop: usedDropScore({usedDropScore}) + dropScore({item.dropScore}) > maxDropScore({maxDropScore})");
                    break;
                }

                usedDropScore += item.dropScore;
                finalCount++;
                Debug.Log($"  Guaranteed drop #{i + 1}: total usedDropScore = {usedDropScore}");
            }

            // 추가 확률 드랍
            if (leftoverChance > 0f &&
                usedDropScore + item.dropScore <= maxDropScore)
            {
                float roll = Random.Range(0f, 100f);
                Debug.Log($"  Attempting leftover drop: chance = {leftoverChance}, rolled = {roll}");

                if (roll < leftoverChance)
                {
                    usedDropScore += item.dropScore;
                    finalCount++;
                    Debug.Log($"  Success! Leftover drop added. Total usedDropScore = {usedDropScore}");
                }
                else
                {
                    Debug.Log($"  Failed leftover drop.");
                }
            }

            if (finalCount > 0)
            {
                int index = result.FindIndex(x => x.rankType == item.rank);
                if (index >= 0)
                {
                    result[index] = new LootingData(item.rank, result[index].count + finalCount);
                    Debug.Log($"  Updated existing entry: {item.rank} → count = {result[index].count}");
                }
                else
                {
                    result.Add(new LootingData(item.rank, finalCount));
                    Debug.Log($"  Added new entry: {item.rank} → count = {finalCount}");
                }
            }
            else
            {
                Debug.Log($"  No loot given for item: {item.name}");
            }
        }

        Debug.Log($"[LootSystem] Final result count: {result.Count}");
        foreach (var data in result)
        {
            Debug.Log($" → {data.rankType}: {data.count}");
        }

        return result;
    }

    /*
    public List<LootingData> CalculateGameEndingLoots(int totalScore)
    {
        var result = new List<LootingData>();
        int usedDropScore = 0;

        // 드랍 점수 높은 순서대로 리스트 가져오기.
        var lootItems = BoxSlotManager.Instance.AllBoxDataList.OrderByDescending(item => item.dropScore);
        Debug.Log($"lootItems Count: {BoxSlotManager.Instance.AllBoxDataList.Count}");

        foreach (var item in lootItems)
        {
            if (totalScore < item.minScore)
                continue;

            // 2. 누적 확률 계산
            int bonusCount = Mathf.Max(0, (totalScore - item.minScore) / item.bonusScoreStep);
            float totalChance = item.baseRate + (bonusCount * item.bonusRate);

            int guaranteedCount = Mathf.FloorToInt(totalChance / 100f);
            float leftoverChance = totalChance % 100f;

            int finalCount = 0;

            // 3. 확정 획득 계산 (드랍 점수 한도 내에서만)
            for (int i = 0; i < guaranteedCount; i++)
            {
                if (usedDropScore + item.dropScore > maxDropScore)
                    break;

                usedDropScore += item.dropScore;
                finalCount++;
            }

            // 4. 나머지 확률로 추가 획득 시도
            if (leftoverChance > 0f &&
                usedDropScore + item.dropScore <= maxDropScore &&
                Random.Range(0f, 100f) < leftoverChance)
            {
                usedDropScore += item.dropScore;
                finalCount++;
            }

            if (finalCount > 0)
            {
                // 기존 데이터에 누적
                var existing = result.Find(x => x.rankType == item.rank);
                if (result.Any(x => x.rankType == item.rank))
                {
                    existing.count += finalCount;
                    result.RemoveAll(x => x.rankType == item.rank);
                    result.Add(existing);
                }
                else
                {
                    result.Add(new LootingData(item.rank, finalCount));
                }
            }
        }

        return result;
    }
    */

    public void TestLoot()
    {
        AddBox(BoxRankType.Normal);
    }
}
