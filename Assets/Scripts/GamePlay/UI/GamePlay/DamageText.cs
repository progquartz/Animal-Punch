using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText; // TextMeshProUGUI 컴포넌트
    public float moveUpDistance = 50f;   // 위로 이동할 거리
    public float duration = 1f;          // 애니메이션 지속 시간
    public float scalePunch = 0.2f;      // 펀치 효과 크기
    public float scaleDuration = 1.0f;   // 펀치 효과 지속 시간

    private RectTransform rectTransform;
    private DamageTextPooler pool;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetPool(DamageTextPooler pool)
    {
        this.pool = pool;
    }
    public void ShowDamage(float damage, Vector3 worldPosition)
    {
        damageText.text = ((int)damage).ToString();

        Vector3 offset = new Vector3(Random.Range(-20f, 20f), Random.Range(20f, 40f), 0f);
        //Vector3 offset = new Vector3(0f,0f, 0f);
        rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
        //rectTransform.position = worldPosition + offset;

        // 투명도 초기화
        rectTransform.localScale = Vector3.one;
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1f);

        // Dotween으로 위로 이동.
        rectTransform.DOMoveY(rectTransform.position.y + moveUpDistance, duration).SetEase(Ease.OutQuad);
        damageText.DOFade(0f, duration).SetEase(Ease.Linear).OnComplete(() => {
            pool.ReturnToPool(this);// 풀로 반환
        });
        rectTransform.DOPunchScale(Vector3.one * scalePunch, scaleDuration, 1, 1f);
    }
}
