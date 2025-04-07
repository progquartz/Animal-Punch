using TMPro;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    [SerializeField] private PlayerPhysics playerMove;
    [SerializeField] private TMP_Text speedText;

    private void Update()
    {
        speedText.text = $"Speed : {(int)Player.Instance.Stat.RigidbodySpeed} km/h";
    }
}
