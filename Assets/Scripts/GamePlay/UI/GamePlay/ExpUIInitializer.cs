using UnityEngine;
using UnityEngine.UI;

public class ExpUIInitializer : MonoBehaviour
{
    [SerializeField] private RatioUI ratioUI;
    void Start()
    {
        ratioUI.GetRatio = () =>  (float)Player.Instance.Stat.currentExp / Player.Instance.Stat.levelUpExpNeed;
    }
}
