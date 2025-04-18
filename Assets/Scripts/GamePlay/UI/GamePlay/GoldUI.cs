using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    private float disappearingTime = 3.0f;
    public TMP_Text goldText;


    // Update is called once per frame
    void Update()
    {
        goldText.text = Player.Instance.Inventory.gold.ToString();
    }
}
