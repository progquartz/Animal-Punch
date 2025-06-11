using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnLockSlotUI : MonoBehaviour
{
    public Button SelectButton;
    public string unlockableId;
    public Image AnimalImage;
    public TMP_Text AnimalText;
    public Image unlockcostImage;
    public TMP_Text SelectButtonText;


    public Sprite gemSprite;
    public Sprite goldSprite;

    public void ChangeUI(bool isUnlocked, bool isSelected, UnlockableDataSO unlockData)
    {
        unlockableId = unlockData.id;
        AnimalImage.sprite = unlockData.icon;
        
        if(isUnlocked)
        {
            AnimalText.text = unlockData.displayName;
            AnimalImage.color = Color.white;
            unlockcostImage.gameObject.SetActive(false);

            if(isSelected)
            {
                SelectButtonText.text = "Selected";
                SelectButton.interactable = false;
            }
            else
            {
                SelectButtonText.text = "Select";
                SelectButton.interactable = true;
            }
        }
        else
        {
            AnimalText.text = "???";
            SelectButtonText.text = $"{unlockData.costType.cost}";
            unlockcostImage.gameObject.SetActive(true);
            AnimalImage.color = Color.black;
            if (unlockData.costType.costType == CostType.Gem)
            {   
                unlockcostImage.sprite = gemSprite;
            }
            else
            {
                unlockcostImage.sprite = goldSprite;
            }
        }
    }

    

}
