using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoxInventoryManager : SingletonBehaviour<BoxInventoryManager>
{
    public BoxInventory inventory = new();
    private string savePath => Path.Combine(Application.persistentDataPath, "boxinventory.json");

    protected override void Init()
    {
        base.Init();
        Load();
    }

    public void AddBox(string boxId)
    {
        inventory.Add(boxId);
        Save();
    }

    public bool AssignBox(int inventoryIndex, int slotIndex)
    {
        string keyTryOpening = inventory.GetKey(inventoryIndex);
        bool result = inventory.Use(inventoryIndex);
        if(result && keyTryOpening != null)
        {
            BoxDataSO boxData = BoxSlotManager.Instance.allBoxDataList.Find(b => b.id == keyTryOpening);
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

    public void TestLoot()
    {
        AddBox("NormalBox");
    }
}
