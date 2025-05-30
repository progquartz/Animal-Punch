using UnityEngine;

[System.Serializable]
public class PlayerInfoDatas : MonoBehaviour
{
    public string playerName;
    public int playerHighScore;

    public int currentLevel; // 현재 레벨
    public int currentExp; // 현재 경험치
    public int nextExp; // 레벨업 필요 경험치

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

    public void OnGameEndResult(int score)
    {
        LoadHighScore();

        if(score >= playerHighScore)
        {
            SaveHighScore(score);
        }
    }

   
    public void GainGold(int amount)
    {
        LoadGoodsData();

        gold += amount;

        SaveGoodsData();
    }

    public void GainGen(int amount)
    {
        LoadGoodsData();

        gem += amount;

        SaveGoodsData();
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

    private void OnLevelUp()
    {
        int expLeft = currentExp - nextExp;
        currentLevel++;
        currentExp = expLeft;
    }


    private void SaveExpData()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerCurrentExp", currentExp);
    }

    private void LoadExpData()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentExp = PlayerPrefs.GetInt("PlayerCurrentExp", 0);

        nextExp = DataManager.Instance.PlayerDataStorage.ExpDatas[currentLevel];
    }

    private void SaveGoodsData()
    {
        PlayerPrefs.SetInt("Gold", 0);
        PlayerPrefs.SetInt("Gem", 0);
    }

    private void LoadGoodsData()
    {
        gem = PlayerPrefs.GetInt("Gold", 0);
        gold = PlayerPrefs.GetInt("Gem", 0);
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
