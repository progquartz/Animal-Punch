using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChestGemOpenUI : MonoBehaviour
{
    public GameObject ui;
    public TMP_Text askingText;
    public TMP_Text gemCountText;


    private readonly int warningCycle = 3;
    private readonly float warningTime = 0.3f;
    private bool isGemWarningActive = false;

    private LootingBoxSlotUI caller = null;
    int gemNeedToOpen = -1;

    private bool isActive = false;

    private void Update()
    {
        if(isActive)
        {
            if(!caller.IsSlotOccupied())
            {
                CloseUI();
            }
        }
    }

    public void OpenUI(LootingBoxSlotUI caller,  int needGemCount)
    {
        isActive = true;
        askingText.text = $"Using {needGemCount} Gem To Open Chest?";
        gemCountText.text = $"{needGemCount}";
        ActiveSelf();
        this.caller = caller;
        gemNeedToOpen = needGemCount;
    }

    public void OnClickGemButton()
    {
        if(GameManager.Instance.GetPlayerInfoData().gem >= gemNeedToOpen)
        {
            // 열면됨.
            caller.OpenBox(true);
            CloseUI(true);
        }
        else
        {
            if(!isGemWarningActive)
            {
                isGemWarningActive = true;
                StartCoroutine(ColorWarningCoroutine());
            }
            SoundManager.Instance.PlaySFX("LootingWarning", AudioType.UI);
        }
        
    }
    
    private void ActiveSelf()
    {
        ui.SetActive(true);
    }

    public void OnClickCloseButton()
    {
        CloseUI();
    }

    private void CloseUI(bool isCallingAdditionalUI = false)
    {
        isActive = false;
        caller = null;
        gemNeedToOpen = -1;
        isGemWarningActive = false;
        if(!isCallingAdditionalUI)
        {
            TitleUI titleUI = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
            titleUI.OnAdditionalUIToggled(false);
        }
        ui.SetActive(false);
    }

    private IEnumerator ColorWarningCoroutine()
    {
        bool isWhite = true;

        for (int i = 0; i < warningCycle; i++)
        {
            gemCountText.color = isWhite ? Color.red : Color.white;
            isWhite = !isWhite;
            yield return new WaitForSeconds(warningTime);
        }

        // 마지막에는 흰색으로 초기화해줌 (선택사항)
        gemCountText.color = Color.white;
        isGemWarningActive = false;
    }
}
