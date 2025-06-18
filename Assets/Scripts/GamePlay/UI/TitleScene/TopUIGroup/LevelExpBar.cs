using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelExpBar : MonoBehaviour
{
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private TMP_Text ExpText;
    [SerializeField] private Image ExpBar;
    [SerializeField] private Vector3 originalScale;

    void Start()
    {
        originalScale = ExpBar.transform.localScale;
    }

    private void Update()
    {
        PlayerInfoDatas infoData = GameManager.Instance.GetPlayerInfoData();
        UpdateLevelText(infoData);
        UpdateExpText(infoData);
        UpdateExpBar(infoData);
    }

    private void UpdateExpBar(PlayerInfoDatas infoData)
    {
        if (GameManager.Instance.IsGamePaused) return;
        
        float currentExp = (float)infoData.currentExp;
        float nextExp = (float)infoData.nextExp;
        float ratio = currentExp / nextExp;
        
        ExpBar.transform.localScale = new Vector3(
            originalScale.x * ratio,
            originalScale.y,
            originalScale.z);
    }

    private void UpdateLevelText(PlayerInfoDatas infoData)
    {
        LevelText.text = infoData.currentLevel.ToString();
    }

    private void UpdateExpText(PlayerInfoDatas infoData)
    {
        ExpText.text = $"{infoData.currentExp} / {infoData.nextExp}";

    }
}
