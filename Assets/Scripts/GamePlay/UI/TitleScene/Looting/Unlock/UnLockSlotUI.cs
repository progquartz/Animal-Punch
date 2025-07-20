using System.Collections;
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

    private readonly int warningCycle = 3;
    private readonly float warningTime = 0.3f;
    private bool isGemWarningActive = false;


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

    public void OnBuyFailed()
    {
        SoundManager.Instance.PlaySFX("LootingWarning", AudioType.UI);
        if(!isGemWarningActive)
        {
            isGemWarningActive = true;
            StartCoroutine(ColorWarningCoroutine());
        }
        
    }

    public void OnBuySuceed()
    {
        SoundManager.Instance.PlaySFX("Purchase", AudioType.UI);
    }


    private IEnumerator ColorWarningCoroutine()
    {
        bool isWhite = true;

        for (int i = 0; i < warningCycle; i++)
        {
            SelectButtonText.color = isWhite ? Color.red : Color.white;
            isWhite = !isWhite;
            yield return new WaitForSeconds(warningTime);
        }

        // 마지막에는 흰색으로 초기화해줌 (선택사항)
        SelectButtonText.color = Color.white;
        isGemWarningActive = false;
    }

}
