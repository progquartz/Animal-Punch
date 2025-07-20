using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class PlayerInfoUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public Image playerImage;
    public Image playerRankImage;
    public TMP_Text playerHighScore;

    public RankData[] rankDatas;

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
        return  DataManager.Instance.PlayerDataStorage.GetRankData(score).rankSprite;
    }


    public void OnClickPlayerRecordButton()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        OverlayCanvas.instance.CallUnderDevelopmentUI();
    }
    
}
