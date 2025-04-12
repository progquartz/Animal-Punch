using UnityEngine;
using UnityEngine.UI;

public class ActorsHpBar : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Enemy owner;


    public RectTransform HpBarTransform;
    public RectTransform HpBar;
    public RectTransform HpBackground;



    private void Start()
    {
        cam = Camera.main;
        owner.OnInit += ShowHpBar;
        owner.OnDead += HideHpBar;
    }

    private void ShowHpBar()
    {
        HpBarTransform?.gameObject.SetActive(true);
    }

    private void HideHpBar()
    {
        HpBarTransform?.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateHealthBarRotation();
    }

    private void UpdateHealthBar()
    {
        float ratio = owner.stat.HP / owner.stat.MaxHP;
        float newWidth = HpBackground.rect.width * ratio;
        HpBar.sizeDelta = new Vector2(newWidth, HpBar.sizeDelta.y);
    }

    private void UpdateHealthBarRotation()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);
    }
}

