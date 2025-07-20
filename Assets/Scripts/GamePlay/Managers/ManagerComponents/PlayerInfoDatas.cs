using JetBrains.Annotations;
using Mono.Cecil;
using UnityEngine;

[System.Serializable]
public class PlayerInfoDatas : MonoBehaviour
{
    public string playerName;
    public int playerHighScore;

    public int currentLevel; // 현재 레벨
    public int currentExp; // 현재 경험치
    public int nextExp; // 레벨업 필요 경험치
    public int queuedExp;

    public int gold; // 골드
    public int gem; // 잼
    

    public void Init()
    {
        LoadExpData();
        LoadGoodsData();
        LoadHighScore();
        LoadPlayerName();
    }

    public void ChangePlayerName(string name)
    {
        LoadPlayerName();

        playerName = name;

        SavePlayerName(playerName);
    }

    public void UpdateGameEndResult(int score)
    {
        LoadHighScore();

        if(score >= playerHighScore)
        {
            SaveHighScore(score);
        }
    }

   
    public void GainGold(int amount, bool needsSave = false)
    {
        gold += amount;

        Debug.Log($"Gold = {gold}를 추가합니다.");
        if (needsSave)
        {
            SaveGoodsData();
            Debug.Log($"Gold = {gold}를 저장합니다.");
        }
    }
    public void GainGem(int amount)
    {
        gem += amount;
        Debug.Log($"Gem = {gem}를 저장합니다.");
        SaveGoodsData();
    }

    public bool TryUseCosts(CostType costType, int amount)
    {
        if(costType == CostType.Gem)
        {
            return TryUseGem(amount);
        }
        else 
        {
            return TryUseGold(amount);
        }
    }
    private bool TryUseGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;
            SaveGoodsData() ;
            return true;
        }
        return false;
    }

    private bool TryUseGem(int amount)
    {
        if (gem >= amount)
        {
            gem -= amount;
            SaveGoodsData();
            return true;
        }
        return false;
    }


    /// <summary>
    /// 만약 레벨업 한다면 true 리턴
    /// </summary>
    public void GainExp(int amount)
    {
        LoadExpData();

        currentExp += amount;

        if (currentExp >= nextExp)
        {
            OnLevelUp();
        }

        SaveExpData();
    }

    public void QueueGainExp(int amount)
    {
        queuedExp += amount;
        Debug.Log($"ExpGained {queuedExp}");
        PlayerPrefs.SetInt("QueuedExp", queuedExp);
        PlayerPrefs.Save();
    }

    public void LoadQueuedExp()
    {
        queuedExp = PlayerPrefs.GetInt("QueuedExp", 0);
    }

    public void ClearQueuedExp()
    {
        queuedExp = 0;
        PlayerPrefs.SetInt("QueuedExp", 0);
    }

    public void ClearExp()
    {
        currentExp = 0;
        currentLevel = 0;
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerCurrentExp", currentExp);
        PlayerPrefs.Save();
    }

    private void OnLevelUp()
    {
        int expLeft = currentExp - nextExp;
        currentLevel++;
        currentExp = expLeft;
    }


    public void SaveExpData()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerCurrentExp", currentExp);
        PlayerPrefs.Save();
    }

    private void LoadExpData()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentExp = PlayerPrefs.GetInt("PlayerCurrentExp", 0);

        nextExp = DataManager.Instance.PlayerDataStorage.ExpDatas[currentLevel];
    }

    private void SaveGoodsData()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Gem", gem);
    }

    private void LoadGoodsData()
    {
        gem = PlayerPrefs.GetInt("Gem", 0);
        gold = PlayerPrefs.GetInt("Gold", 0);
    }

    private void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt("PlayerHighScore", score);
    }

    private void LoadHighScore()
    {
        playerHighScore = PlayerPrefs.GetInt("PlayerHighScore", 0);
    }

    private void SavePlayerName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    private void LoadPlayerName()
    {
        int randomNum = Random.Range(100, 100000);
        string tempName = $"Player{randomNum}";
        playerName = PlayerPrefs.GetString("PlayerName", tempName);
        if(playerName == tempName )
        {
            SavePlayerName(tempName);
        }
    }


}
