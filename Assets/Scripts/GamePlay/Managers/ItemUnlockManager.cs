using System.IO;
using System.Net.Security;
using UnityEngine;

public class ItemUnlockManager : SingletonBehaviour<ItemUnlockManager> 
{

    public PlayerUnlockData playerData = new PlayerUnlockData();
    private string savePath;

    protected override void Init()
    {
        base.Init();
        savePath = Path.Combine(Application.persistentDataPath, "unlocks.json");
        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            playerData = JsonUtility.FromJson<PlayerUnlockData>(json);
        }
        else
        {
            playerData = new PlayerUnlockData();
            Save(); // 초기화 후 저장
        }
    }

    public void UnlockItem(string id)
    {
        playerData.Unlock(id);
        Save();
    }

    public bool IsUnlocked(string id)
    {
        return playerData.IsUnlocked(id);
    }
}
