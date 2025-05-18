using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class BoostUI : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Player owner;
    [SerializeField] private Image boosterImage;
    [SerializeField] private Image boosterImageBackground;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private Vector3 hpBarOffset = new Vector3(-0.5f, 0f, -2f);
    private Vector3 OriginalSize = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        OriginalSize = boosterImage.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBarPosition();
        UpdateHealthBarRotation();
    }
    private void UpdateHealthBarPosition()
    {
        boosterImage.transform.position = owner.PlayerTransform.position + hpBarOffset;
        boosterImageBackground.transform.position = owner.PlayerTransform.position + hpBarOffset;
        float ratio = owner.Stat.BoostChargeRatio;
        ratio = Mathf.Clamp01(ratio); // ratio는 0~1로 제한
        SetUIColorByRatio(ratio);
        SetUISizeByRatio(ratio);
    }

    private void UpdateHealthBarRotation()
    {
        boosterImage.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);
        boosterImageBackground.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                 cam.transform.rotation * Vector3.up);
    }


    private void SetUISizeByRatio(float ratio)
    {
        boosterImage.transform.localScale = new Vector3(OriginalSize.x * ratio, OriginalSize.y, OriginalSize.z);
    }
    public void SetUIColorByRatio(float ratio)
    {

        boosterImage.color = Color.Lerp(startColor, endColor, ratio);
    }
}
