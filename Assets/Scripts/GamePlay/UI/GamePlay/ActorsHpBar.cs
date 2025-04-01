using UnityEngine;
using UnityEngine.UI;

public class ActorsHpBar : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private ActorsStat targetStat;


    public RectTransform HpBarTransform;
    public RectTransform HpBar;
    public RectTransform HpBackground;



    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateHealthBarRotation();
    }

    private void UpdateHealthBar()
    {
        float ratio = targetStat.HP / targetStat.MaxHP;
        float newWidth = HpBackground.rect.width * ratio;
        HpBar.sizeDelta = new Vector2(newWidth, HpBar.sizeDelta.y);
    }

    private void UpdateHealthBarRotation()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);
    }
}

