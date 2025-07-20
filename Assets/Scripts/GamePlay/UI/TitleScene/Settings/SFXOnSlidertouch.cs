using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class SFXOnSlidertouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Slider slider;
    private bool isDragging = false;
    private float lastValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        lastValue = slider.value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        if (Mathf.Abs(slider.value - lastValue) > Mathf.Epsilon)
        {
            PlaySFX();
            lastValue = slider.value;
        }
    }

    private void PlaySFX()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
    }
}
