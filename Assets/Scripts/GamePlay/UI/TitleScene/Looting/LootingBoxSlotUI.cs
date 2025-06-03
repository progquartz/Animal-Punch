using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

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
    private float refreshCycle = 1.0f;


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
                openText.text = "999";
                timerText.text = $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
                gemIcon.gameObject.SetActive(true);
            }
        }
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
            BoxSlotManager.Instance.TryOpenSlot(slotIndex);
        }
    }

    // 비어있다면, 채워넣는거 고르는 선택지 열리게.
    public void OnClickSlotButton()
    {
        if (slot == null || slot.IsOccupied) return;

        // slot inventory 열리고, chest 선택할 수 있게.
    }
}
