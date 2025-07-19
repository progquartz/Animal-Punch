using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class LootingBoxSlotUI : MonoBehaviour
{
    public int slotIndex;                 // 이 UI가 참조할 슬롯 인덱스
    public GameObject BoxComponents;
    public TMP_Text openText;
    public TMP_Text timerText;
    public Image boxIcon;
    public Image gemIcon;

    private LootingBoxSlot slot;

    private bool isActive = false;
    


    private float refreshTime = 0f;
    private float refreshCycle = 0.5f;

    private int minutePerGem = 120;

    [SerializeField]
    private ChestGemOpenUI chestGemOpenUI;


    void OnEnable()
    {
        Refresh();
    }

    void Update()
    {
        RefreshAndUpdateEachSecond();

    }

    private void RefreshAndUpdateEachSecond()
    {
        if (slot == null) return;

        refreshTime += Time.deltaTime;
        if (refreshTime > refreshCycle)
        {
            refreshTime = 0f;

            Refresh();

            if (!isActive) return;

            if (slot.IsComplete())
            {
                openText.text = "Open";
                timerText.text = "00:00:00";
                gemIcon.gameObject.SetActive(false);
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(slot.RemainingSeconds());
                openText.text = GetGemNeedCount(ts).ToString();
                timerText.text = $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
                gemIcon.gameObject.SetActive(true);
            }
        }
    }

    public int GetGemNeedCount(TimeSpan ts)
    {
        return ((int)(ts.TotalSeconds / minutePerGem));
    }
    

    public void Refresh()
    {
        slot = BoxSlotManager.Instance.GetSlot(slotIndex);

        if (!slot.IsOccupied)
        {
            BoxComponents.SetActive(false);
            isActive = false;
            return;
        }

        boxIcon.sprite = slot.boxData.icon;

        BoxComponents.SetActive(true);
        isActive = true;
    }

    public void OnClickOpenButton()
    {
        if (!isActive || slot == null) return;

        
        if (slot.IsComplete())
        {
            // 상자 열기 가능.
            OpenBox();
        }
        else
        {
            // 잼이 있으면 잼으로 열건지 알려주는 UI 뜨고...
            TimeSpan ts = TimeSpan.FromSeconds(slot.RemainingSeconds());
            int gemCount = GetGemNeedCount(ts);
            chestGemOpenUI.OpenUI(this, gemCount);
            TitleUI titleUI = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
            titleUI.OnAdditionalUIToggled(true);
        }
    }

    public void OpenBox(bool isUsedGem = false)
    {
        Debug.Log("Try Open Box");
        BoxSlotManager.Instance.TryOpenSlot(slotIndex, isUsedGem);
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
    }


    // 비어있다면, 채워넣는거 고르는 선택지 열리게.
    public void OnClickSlotButton()
    {
        if (slot == null || slot.IsOccupied) return;

        BoxSlotManager.Instance.slotButtonRequestIndex = slotIndex;
        // slot inventory 열리고, chest 선택할 수 있게.
        UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
        TitleUI titleUI = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        titleUI.OnAdditionalUIToggled(true);
    }

    public bool IsSlotOccupied()
    {
        return slot.IsOccupied;
    }
}
