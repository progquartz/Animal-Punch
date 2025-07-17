using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnderDevelopmentUI : MonoBehaviour
{
    public Image maskedImage; 
    public TMP_Text maskedText;  

    private float revealDuration = 0.2f;
    private float waitingTime = 1f;
    private float hideDuration = 0.2f;

    private RectTransform imageRectTransform;
    
    private float originalHeight;

    bool isUICalled = false;
    private Coroutine currentlyCallingCoroutine;

    private void Awake()
    {
        imageRectTransform = maskedImage.GetComponent<RectTransform>();
        originalHeight = imageRectTransform.sizeDelta.y;

        SetImageHeight(0f);
    }

    private void SetImageHeight(float height)
    {
        Vector2 size = imageRectTransform.sizeDelta;
        size.y = height;
        imageRectTransform.sizeDelta = size;
    }

    public void StartReveal()
    {
        if (isUICalled)
        {
            StopCoroutine(currentlyCallingCoroutine);
            currentlyCallingCoroutine = null;
            SetImageHeight(0f);
        }
        currentlyCallingCoroutine = StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        isUICalled = true;
        float elapsed = 0f;
        while (elapsed < revealDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / revealDuration);
            SetImageHeight(Mathf.Lerp(0f, originalHeight, t));
            yield return null;
        }

        SetImageHeight(originalHeight);
        // ´ë±â
        yield return new WaitForSeconds(waitingTime);

        elapsed = 0f;
        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / hideDuration);
            SetImageHeight(Mathf.Lerp(originalHeight, 0f, t));
            yield return null;
        }

        SetImageHeight(0f);
        isUICalled = false;
    }
}
