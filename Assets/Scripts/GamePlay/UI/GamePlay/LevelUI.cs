using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public TMP_Text levelText;
    private string baseString = "Level :";

    // Update is called once per frame
    void Update()
    {
        levelText.text = baseString + Player.Instance.Stat.level.ToString();
    }
}
