using System.Collections.Generic;

[System.Serializable]
public class PlayerUnlockData
{
    public List<string> unlockedItemIds = new List<string>();

    public bool IsUnlocked(string id) => unlockedItemIds.Contains(id);

    public void Unlock(string id)
    {
        if (!IsUnlocked(id))
            unlockedItemIds.Add(id);
    }
}
