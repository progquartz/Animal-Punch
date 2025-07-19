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
    private bool isTimeWarningActive = false;

    private int warningCycle = 3;
    private float warningTime = 0.3f;

    private float refreshTime = 0f;
    private float refreshCycle = 0.5f;

    private int minutePerGem = 120;


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
                openText.text = ((int)(ts.TotalSeconds / minutePerGem)).ToString();
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
            Debug.Log("Try Open Box");
            BoxSlotManager.Instance.TryOpenSlot(slotIndex);
            SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        }
        else
        {
            // 시간 오류나는거 알려주기.
            if(!isTimeWarningActive)
            {
                isTimeWarningActive = true;
                StartCoroutine(ColorWarningCoroutine());
                SoundManager.Instance.PlaySFX("LootingWarning", AudioType.UI);
            }
        }
    }

    private IEnumerator ColorWarningCoroutine()
    {
        bool isWhite = true;

        for (int i = 0; i < warningCycle; i++)
        {
            timerText.color = isWhite ? Color.red : Color.white;
            isWhite = !isWhite;
            yield return new WaitForSeconds(warningTime);
        }

        // 마지막에는 흰색으로 초기화해줌 (선택사항)
        timerText.color = Color.white;
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
}
