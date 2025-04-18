using UnityEngine;
using UnityEngine.UI;

public class GameTimeUIInitializer : MonoBehaviour
{
    [SerializeField] private RatioUI ratioUI;
    void Start()
    {
        ratioUI.GetRatio = () => GameManager.Instance.GameTimeLeft / GameManager.Instance.MaxGameTime;
    }
}
