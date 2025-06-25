using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameOverScreenUI : MonoBehaviour
{
    public TMP_Text totalScoreText;
    public TMP_Text timeScoreText;

    public TMP_Text enemyScoreText;
    public TMP_Text floatingScoreTextPrefab;
    public Transform floatingTextParent;


    private int currentTotalScore = 0;
    private int targetTimeScore = 0;
    private int currentTimeScore = 0;
    private int currentEnemyScore = 0;
    public float scoreAnimationSpeed = 50f;

    public int[] scoreMilestones = { 5000, 10000, 20000 };

    public bool isTriggered = false;
    public bool isSkipped = false;

    private float currentTime = 0f;
    private float droppingTime = 0f;


    private void Update()
    {
        if (!isTriggered) return;

        // 시간 점수 처리
        if (!isSkipped)
        {
            if (currentTimeScore < targetTimeScore)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / droppingTime);
                currentTimeScore = Mathf.RoundToInt(Mathf.Lerp(0, targetTimeScore, t));
            }
        }
        else
        {
            // 스킵된 경우에는 즉시 최대값으로 설정
            currentTimeScore = targetTimeScore;
            currentEnemyScore = GameManager.Instance.EnemyDeathCountHandler.GetTotalDeathCount(); // 예시
        }

        // 총합 갱신
        currentTotalScore = currentTimeScore + currentEnemyScore;

        // 텍스트 업데이트
        timeScoreText.text =  "Time : " + currentTimeScore.ToString("N0");
        enemyScoreText.text = "Enemy : " + currentEnemyScore.ToString("N0");
        totalScoreText.text = currentTotalScore.ToString("N0");
    }


    public void ShowScore(float dropTime)
    {
        isTriggered = true;
        isSkipped = false;
        droppingTime = dropTime;
        currentTime = 0f;

        targetTimeScore = Mathf.RoundToInt(GameManager.Instance.GameTime);
    }

    public void OnSkipButtonTriggered()
    {
        isSkipped = true;
    }

    public void AddEnemyScore(int score)
    {
        StartCoroutine(AnimateEnemyScore(score));
        SpawnFloatingScore(score);
    }

    public void AddEnemyScoreSkipped(int score)
    {
        int targetScore = currentEnemyScore + score;
        enemyScoreText.text = targetScore.ToString();
    }

    public void OnAddingEnemyScoreFinished()
    {
        
    }

    private IEnumerator AnimateEnemyScore(int score)
    {
        int targetScore = currentEnemyScore + score;

        while (currentEnemyScore < targetScore)
        {
            currentEnemyScore += Mathf.CeilToInt(scoreAnimationSpeed * Time.deltaTime);
            if (currentEnemyScore > targetScore) currentEnemyScore = targetScore;

            enemyScoreText.text = currentEnemyScore.ToString("N0");
            CheckMilestones(currentEnemyScore);

            yield return null;
        }
    }

    private void SpawnFloatingScore(int score)
    {
        TMP_Text floatingScore = Instantiate(floatingScoreTextPrefab, floatingTextParent);
        floatingScore.text = $"+{score:N0}";
        Destroy(floatingScore.gameObject, 1.5f);
    }

    private void CheckMilestones(int score)
    {
        foreach (var milestone in scoreMilestones)
        {
            if (currentEnemyScore >= milestone && (currentEnemyScore - milestone) < scoreAnimationSpeed)
            {
                PlayMilestoneEffect();
                break;
            }
        }
    }

    private void PlayMilestoneEffect()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        StartCoroutine(ScaleScoreTextRoutine());
    }

    private IEnumerator ScaleScoreTextRoutine()
    {
        Vector3 originalScale = enemyScoreText.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f;

        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            enemyScoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            enemyScoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            yield return null;
        }

        enemyScoreText.transform.localScale = originalScale;
    }
}
