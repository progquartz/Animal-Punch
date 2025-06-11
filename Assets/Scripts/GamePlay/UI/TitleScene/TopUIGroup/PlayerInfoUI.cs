using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public Image playerImage;
    public Image playerRankImage;
    public TMP_Text playerHighScore;

    public Sprite[] rankImages;

    private void Update()
    {
        PlayerInfoDatas infoData = GameManager.Instance.GetPlayerInfoData();
        UpdatePlayerInfo(infoData);
        UpdatePlayerScoreRank(infoData);
    }

    private void UpdatePlayerInfo(PlayerInfoDatas infoData)
    {
        playerNameText.text = infoData.playerName;

    }

    private void UpdatePlayerScoreRank(PlayerInfoDatas infoData)
    {
        playerHighScore.text = infoData.playerHighScore.ToString();
        UpdatePlayerScoreRankImage(infoData);
    }

    private void UpdatePlayerScoreRankImage(PlayerInfoDatas infoData)
    {
        int score = infoData.playerHighScore;
        playerRankImage.sprite = GetRankImage(score);
    }

    private Sprite GetRankImage(int score)
    {
        int index = score % 100;
        if(index > 8)
        {
            index = 8;
        }
        return rankImages[index];
    }

    
}
