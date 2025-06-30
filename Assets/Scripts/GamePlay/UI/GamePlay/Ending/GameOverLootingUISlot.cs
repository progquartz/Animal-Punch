using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLootingUISlot : MonoBehaviour
{
    public Image LootingUIBackground;
    public Image LootingUIForeground;
    public Image LootingItemImage;
    public TMP_Text LootingCountText;
    public Transform LootingSlotTransform;
    private float slotOpenTime = 0.3f;
    private float currentTime = 0.0f;

    private void Update()
    {
        if(LootingItemImage != null)
        {
            currentTime += Time.deltaTime;

            if (currentTime < slotOpenTime)
            {
                float t = currentTime / slotOpenTime;
                t = t * t * (3f - 2f * t);
                Vector3 yChangingScale = new Vector3(1, Mathf.Lerp(0, 1, t), 1);
                LootingSlotTransform.localScale = yChangingScale;
            }
        }
    }

    public void SetUI(LootingRankDesignTemplate template,  int count)
    {
        LootingUIBackground.color = template.backgroundColor;
        LootingUIForeground.color = template.foregroundColor;
        LootingItemImage.sprite = template.sprite;
        LootingCountText.text = count.ToString();
        LootingSlotTransform.localScale = Vector3.zero;
    }


}
