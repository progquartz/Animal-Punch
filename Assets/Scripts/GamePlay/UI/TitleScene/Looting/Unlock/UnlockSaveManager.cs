using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

public class UnlockSaveManager : SingletonBehaviour<UnlockSaveManager>
{
    public List<string> unlockedIds = new();
    public string selectedIds;
    public List<UnlockableDataSO> allUnlocks = new();
    public bool hasChange = false;
    private string unlockIdSavePath => Path.Combine(Application.persistentDataPath, "unlocked.json");
    private string allUnlockListPath = "ScriptableObjects/LootingData/";

    protected override void Init()
    {
        base.Init();
        LoadAllIds();
        LoadUnlockedIds();
        Debug.Log($"{selectedIds} ??"); 
    }

    public bool IsUnlocked(string id) => unlockedIds.Contains(id);

    public void Unlock(string id)
    {
        if (!IsUnlocked(id))
        {
            unlockedIds.Add(id);
            hasChange = true;
            Save();
        }
    }


    public void ResetAllUnlocks()
    {
        unlockedIds = new List<string>();
        Unlock("Cheetah");
        selectedIds = "Cheetah";
        Save();
    }

    public void ChangeSelected(string id)
    {
        if(selectedIds != id)
        {
            selectedIds = id;
            hasChange = true;
            Save();
        }
    }

    public bool HandleBuyItem(string unlockableId)
    {
        UnlockableDataSO data = GetUnlockData(unlockableId);
        int cost = data.costType.cost;
        CostType costType = data.costType.costType;

        if(GameManager.Instance.GetPlayerInfoData().TryUseCosts(costType, cost))
        {
            Unlock(unlockableId);
            return true;
        }
        return false;
    }

    public void Save()
    {
        File.WriteAllText(unlockIdSavePath, JsonUtility.ToJson(new UnlockDataWrapper { ids = unlockedIds }, true));
        PlayerPrefs.SetString("SelectedCharacterID", selectedIds);
    }

    public void LoadUnlockedIds()
    {
        // unlock id
        if (File.Exists(unlockIdSavePath))
        {
            var wrapper = JsonUtility.FromJson<UnlockDataWrapper>(File.ReadAllText(unlockIdSavePath));
            unlockedIds = wrapper.ids ?? new List<string>();
            Unlock("Cheetah");
        }

        // selected id
        string data = PlayerPrefs.GetString("SelectedCharacterID");
        if(data == null || data == "")
        {
            selectedIds = "Cheetah";
        }
        else
        {
            selectedIds = data;
        }
 
    }

    private void LoadAllIds()
    {
        allUnlocks.Clear();
        UnlockableDataSO[] loadedBoxes = Resources.LoadAll<UnlockableDataSO>(allUnlockListPath);

        if (loadedBoxes != null && loadedBoxes.Length > 0)
        {
            allUnlocks.AddRange(loadedBoxes);
            Debug.Log($"{loadedBoxes.Length}개의 박스 데이터 로드.");

        }
        else
        {
            Debug.LogWarning($"다음 경로에서 박스 데이터를 읽기 실패함. /{allUnlockListPath}");
        }
    }


    public UnlockableDataSO GetUnlockData(string id) => allUnlocks.FirstOrDefault(data => data.id == id);


        // 언락 데이터 저장용 json 클래스.
    [System.Serializable]
    private class UnlockDataWrapper
    {
        public List<string> ids;
    }
}