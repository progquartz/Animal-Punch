using TMPro;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    [SerializeField] private TempPlayerMove playerMove;
    [SerializeField] private TMP_Text speedText;

    private void Update()
    {
        speedText.text = $"Speed : {(int)playerMove.RigidbodySpeed} km/h";
    }
}
