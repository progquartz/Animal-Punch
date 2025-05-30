using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class LootingBoxSlots : MonoBehaviour
{
    public List<LootingBoxSlot> slots = new List<LootingBoxSlot>(4);

    private string savePath => Path.Combine(Application.persistentDataPath, "LootingBoxSlots.json");

    void Start()
    {
        Load();
    }

    public void AssignBoxToSlot(int index, BoxDataSO boxData)
    {
        slots[index] = new LootingBoxSlot
        {
            boxId = boxData.id,
            startTime = DateTime.UtcNow.ToString("o"),
            durationSeconds = boxData.unlockDurationSeconds
        };
        Save();
    }

    public void TryOpenSlot(int index)
    {
        if (slots[index].IsComplete())
        {
            Debug.Log($"상자 {slots[index].boxId} 열림!");
            // 캐릭터 언락 로직 여기에 추가
            slots[index] = new LootingBoxSlot(); // 초기화
            Save();
        }
    }

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
            for (int i = 0; i < 4; i++) slots.Add(new LootingBoxSlot());
            Save();
        }
    }
}
