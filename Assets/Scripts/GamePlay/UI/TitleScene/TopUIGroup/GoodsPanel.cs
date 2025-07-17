using System;
using TMPro;
using UnityEngine;

public class GoodsPanel : MonoBehaviour
{
    public TMP_Text goldText;
    public TMP_Text gemText;

    private void Update()
    {
        PlayerInfoDatas infoData = GameManager.Instance.GetPlayerInfoData();
        UpdateGoldText(infoData);
        UpdateGemText(infoData);
    }

    private void UpdateGemText(PlayerInfoDatas infoData)
    {
        gemText.text = infoData.gem.ToString();
    }

    private void UpdateGoldText(PlayerInfoDatas infoData)
    {
        goldText.text = infoData.gold.ToString();
    }

    public void OnPressGemButton()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        OverlayCanvas.instance.CallUnderDevelopmentUI();
    }

    public void OnPressGoldButton()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        OverlayCanvas.instance.CallUnderDevelopmentUI();
    }
}
