using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText; // TextMeshProUGUI ������Ʈ
    public float moveUpDistance = 50f;   // ���� �̵��� �Ÿ�
    public float duration = 1f;          // �ִϸ��̼� ���� �ð�
    public float scalePunch = 0.2f;      // ��ġ ȿ�� ũ��
    public float scaleDuration = 1.0f;   // ��ġ ȿ�� ���� �ð�

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

        // ���� �ʱ�ȭ
        rectTransform.localScale = Vector3.one;
        damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, 1f);

        // Dotween���� ���� �̵�.
        rectTransform.DOMoveY(rectTransform.position.y + moveUpDistance, duration).SetEase(Ease.OutQuad);
        damageText.DOFade(0f, duration).SetEase(Ease.Linear).OnComplete(() => {
            pool.ReturnToPool(this);// Ǯ�� ��ȯ
        });
        rectTransform.DOPunchScale(Vector3.one * scalePunch, scaleDuration, 1, 1f);
    }
}
