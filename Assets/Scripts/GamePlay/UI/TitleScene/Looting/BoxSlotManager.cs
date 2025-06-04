using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class BoxSlotManager : SingletonBehaviour<BoxSlotManager>
{
    public List<LootingBoxSlot> slots = new(4);
    public List<BoxDataSO> allBoxDataList;

    public int slotButtonRequestIndex = -1;


    private string savePath => Path.Combine(Application.persistentDataPath, "boxslots.json");
    private string boxDataListPath = "ScriptableObjects/TreasureBoxData/";

    protected override void Init()
    {
        Load();
        LoadBoxData();
    }

    public BoxDataSO GetBoxDataById(string id)
    {
        return allBoxDataList.Find(b => b.id == id);
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
        AssignBoxToSlot(0, GetBoxDataById("NormalBox"));
    }


    public void AssignBoxToSlot(int index, BoxDataSO boxData)
    {
        if(allBoxDataList.Count == 0)
            LoadBoxData();


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
            Debug.Log($"상자 {slots[index].boxData.id} 열림!");
            // 캐릭터 언락 로직 여기에 추가
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

    private void LoadBoxData()
    {
        allBoxDataList.Clear();
        BoxDataSO[] loadedBoxes = Resources.LoadAll<BoxDataSO>(boxDataListPath);

        if (loadedBoxes != null && loadedBoxes.Length > 0)
        {
            allBoxDataList.AddRange(loadedBoxes);
            Debug.Log($"{allBoxDataList.Count}개의 박스 데이터 로드.");
            
        }
        else
        {
            Debug.LogWarning($"다음 경로에서 박스 데이터를 읽기 실패함. /{boxDataListPath}");
        }
    }
}
