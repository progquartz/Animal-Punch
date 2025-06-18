using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text levelText2;
    public RectMask2D mask;
    private string baseString = "Level :";

    // Update is called once per frame
    void Update()
    {
        levelText.text = baseString + Player.Instance.Stat.Level.ToString();
        levelText2.text = baseString + Player.Instance.Stat.Level.ToString();
        CalculateMaskPadding();
    }

    private void CalculateMaskPadding()
    {
        float widthSize = levelText.rectTransform.sizeDelta.x;
        float expRatio = (float)Player.Instance.Stat.CurrentExp / Player.Instance.Stat.LevelUpExpNeed;
        mask.padding = new Vector4(expRatio * widthSize, 0,0,0);
    }
}
