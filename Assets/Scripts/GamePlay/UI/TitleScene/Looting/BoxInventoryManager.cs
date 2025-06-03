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

    public void AddBox(string boxId, int amount = 1)
    {
        inventory.Add(boxId, amount);
        Save();
    }

    public bool UseBox(string boxId)
    {
        bool result = inventory.Use(boxId);
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
}
