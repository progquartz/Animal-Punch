using System;
using UnityEngine;
using UnityEngine.UI;

public class RatioUI : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    private Vector3 originalScale;

    // delegate 패턴 사용
    public Func<float> GetRatio;

    void Start()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }
        if (targetImage != null)
        {
            originalScale = targetImage.transform.localScale;
        }
        else
        {
            Debug.LogError("RatioUI에 Image 컴포넌트가 없습니다.");
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;

        if (GetRatio != null)
        {
            float ratio = GetRatio();
            targetImage.transform.localScale = new Vector3(
                originalScale.x * ratio,
                originalScale.y,
                originalScale.z);
        }
    }
}
