using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class BoxSlotManager : SingletonBehaviour<BoxSlotManager>
{
    public List<LootingBoxSlot> slots = new(4);
    public int slotButtonRequestIndex = -1;


    private string savePath => Path.Combine(Application.persistentDataPath, "boxslots.json");
    private string boxDataListPath = "ScriptableObjects/LootingData/TreasureBoxData/";

    protected override void Init()
    {
        base.Init();
        Load();
    }

    public BoxDataSO GetBoxDataById(BoxRankType id)
    {
        return DataManager.Instance.LootingStorage.AllBoxDataList.Find(b => b.rank == id);
    }

    public BoxDataSO GetBoxDataInIndex(int index)
    {
        if (slots[index].IsOccupied)
            return slots[index].boxData;
        else
            return null;
    }

    public void TestBoxPutting()
    {
        AssignBoxToSlot(0, GetBoxDataById(BoxRankType.Normal));
    }


    public void AssignBoxToSlot(int index, BoxDataSO boxData)
    {
        slots[index] = new LootingBoxSlot
        {
            boxData = boxData,
            startTime = DateTime.UtcNow.ToString("o"),
        };
        Save();
    }

    public void TryOpenSlot(int index)
    {
        if (slots[index].IsComplete())
        {
            var box = slots[index].boxData;
            if (box != null && box.unlockableItems != null)
            {
                // 언락... 어떻게시키지 알고리즘 고민.
                foreach (var item in box.unlockableItems)
                {
                    if (!UnlockSaveManager.Instance.IsUnlocked(item.id))
                    {
                        UnlockSaveManager.Instance.Unlock(item.id);
                        Debug.Log($"[언락됨] {item.displayName}");
                        break; // 1개만 언락
                    }
                }
            }

            slots[index] = new LootingBoxSlot(); // 초기화
            Save();
        }
    }

    public bool IsSlotFull()
    {
        bool isSlotFull = true;
        foreach (var slot in slots)
        {
            if(!slot.IsOccupied)
            {
                isSlotFull = false;
                break;
            }
        }
        return isSlotFull;
    }

    public int GetSlotEmpty()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsOccupied)
            {
                return i;
            }
        }
        // 슬롯이 가득 찬 경우.
        return -1;
    }

    public LootingBoxSlot GetSlot(int index) => slots[index];

    public void Save()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            slots.Clear();
            for (int i = 0; i < 4; i++) slots.Add(new LootingBoxSlot());
            Save();
        }
    }
}
