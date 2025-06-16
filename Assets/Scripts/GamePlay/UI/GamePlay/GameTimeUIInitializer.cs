using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeUIInitializer : MonoBehaviour
{
    [SerializeField] private RatioUI ratioUI;
    [SerializeField] private TMP_Text timeLeftText;
    [SerializeField] private Transform timeLeftTextTransform;
    [SerializeField] private Transform targetTransform;

    void Start()
    {
        ratioUI.GetRatio = () => GameManager.Instance.GameTimeLeft / GameManager.Instance.MaxGameTime;
    }

    private void Update()
    {
        timeLeftTextTransform.position = targetTransform.position;
        timeLeftText.text = GameManager.Instance.GameTimeLeft.ToString("F1");
    }
}
