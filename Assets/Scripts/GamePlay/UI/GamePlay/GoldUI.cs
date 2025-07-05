using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TMP_Text goldText;

    void Update()
    {

        goldText.text = GameManager.Instance.GetPlayerInfoData().gold.ToString();
    }
}
