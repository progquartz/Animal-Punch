using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelExpBar : MonoBehaviour
{
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private TMP_Text ExpText;
    [SerializeField] private Image ExpBar;
    [SerializeField] private float barFillDuration = 5.0f;

    private Vector3 originalScale;
    private Coroutine expCoroutine;

    void Start()
    {
        originalScale = ExpBar.transform.localScale;
        Debug.Log("Start beginning");
        PlayerInfoDatas infoData = GameManager.Instance.GetPlayerInfoData();
        infoData.LoadQueuedExp();
        if (infoData.queuedExp > 0)
        {
            Debug.Log("StartExpAnimationStarted");
            StartExpAnimation(infoData);
        }
    }

    void Update()
    {
        if (expCoroutine == null)
        {
            UpdateUI(GameManager.Instance.GetPlayerInfoData());
        }
    }

    public void StartExpAnimation(PlayerInfoDatas infoData)
    {
        if (expCoroutine != null) return;
        expCoroutine = StartCoroutine(AnimateQueuedExp(infoData));
    }


    // 경험치 바 다 달때까지 연속레벨업.
    private IEnumerator AnimateQueuedExp(PlayerInfoDatas infoData)
    {
        int totalExpToGain = infoData.queuedExp;
        infoData.ClearQueuedExp();

        while (totalExpToGain > 0)
        {
            int expToAdd = Mathf.Min(infoData.nextExp - infoData.currentExp, totalExpToGain);
            yield return AnimateOneCycle(infoData, expToAdd);

            totalExpToGain -= expToAdd;

            ApplyLevelUp(infoData);

            UpdateUI(infoData);
            yield return new WaitForSeconds(0.1f);
        }

        infoData.SaveExpData();
        expCoroutine = null;
    }

    // 최대 1개의 레벨업에 관장하는 코루틴
    private IEnumerator AnimateOneCycle(PlayerInfoDatas infoData, int expToAdd)
    {
        int initialExp = infoData.currentExp;
        float duration = barFillDuration * ((float)expToAdd / infoData.nextExp);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float smoothExp = Mathf.Lerp(initialExp, initialExp + expToAdd, t);
            infoData.currentExp = Mathf.FloorToInt(smoothExp);
            UpdateUI(infoData);
            yield return null;
        }

        infoData.currentExp = initialExp + expToAdd;
    }

    private void ApplyLevelUp(PlayerInfoDatas infoData)
    {
        while (infoData.currentExp >= infoData.nextExp)
        {
            int leftover = infoData.currentExp - infoData.nextExp;
            infoData.currentLevel++;
            OpenLevelUpPrize(infoData);
            infoData.currentExp = leftover;
            infoData.nextExp = DataManager.Instance.PlayerDataStorage.ExpDatas[infoData.currentLevel];
        }
    }

    private void OpenLevelUpPrize(PlayerInfoDatas infoData)
    {
        UIManager.Instance.OpenUI<LevelUpLootUI>(new BaseUIData());
        LevelUpLootUI levelUpUI = UIManager.Instance.GetActiveUI<LevelUpLootUI>() as LevelUpLootUI;
        levelUpUI.OpenLoot(infoData.currentLevel);
    }

    private void UpdateUI(PlayerInfoDatas infoData)
    {
        LevelText.text = infoData.currentLevel.ToString();
        ExpText.text = $"{infoData.currentExp} / {infoData.nextExp}";

        float ratio = (float)infoData.currentExp / infoData.nextExp;
        ExpBar.transform.localScale = new Vector3(ratio, originalScale.y, originalScale.z);
    }
}
