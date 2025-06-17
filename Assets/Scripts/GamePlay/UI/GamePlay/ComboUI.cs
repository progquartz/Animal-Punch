using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    public TextMeshProUGUI comboText;
    public Slider comboSlider;
    public GameObject comboUIGroup;

    public PlayerComboHandler comboHandler;

    public float pulseScale = 1.3f;
    public float pulseDuration = 0.2f;

    public Color originalColor = Color.green;
    public Color zeroColor = Color.red;
    private Image fillImage;

    private Vector3 originalScale;
    private Coroutine pulseCoroutine;

    void Start()
    {
        originalScale = comboText.transform.localScale;
        comboHandler = Player.Instance.comboHandler;
        comboUIGroup.SetActive(false);

        fillImage = comboSlider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            fillImage.color = originalColor;
        }
    }

    void Update()
    {
        if (comboHandler.IsComboActive)
        {
            if (!comboUIGroup.activeSelf)
                comboUIGroup.SetActive(true);

            // 텍스트 및 슬라이더 업데이트
            comboText.text = $"{comboHandler.comboCount} Combo!";

            float progress = comboHandler.comboTimer / comboHandler.comboResetTime;
            comboSlider.value = 1f - progress;

            // Fill 색상 업데이트
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(originalColor, zeroColor, progress);
            }
        }
        else
        {
            if (comboUIGroup.activeSelf)
                comboUIGroup.SetActive(false);

            if (fillImage != null)
                fillImage.color = originalColor;
        }
    }

    public void OnComboUpdate()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            comboText.transform.localScale = originalScale;
        }

        pulseCoroutine = StartCoroutine(PulseEffect());
    }

    IEnumerator PulseEffect()
    {
        float elapsed = 0f;

        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            float scale = Mathf.Lerp(1f, pulseScale, Mathf.Sin(t * Mathf.PI));  // 빠르게 커졌다가 원래대로
            comboText.transform.localScale = originalScale * scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        comboText.transform.localScale = originalScale;
        pulseCoroutine = null; // 코루틴 종료 상태 기록
    }
}
