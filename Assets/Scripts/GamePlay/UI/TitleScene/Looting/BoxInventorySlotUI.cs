using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxInventorySlotUI : MonoBehaviour
{
    public int SlotIndex = 0;

    public Button slotButton;
    public Button assignButton;
    
    public BoxDataSO boxData;
    [SerializeField] private TMP_Text boxName;
    [SerializeField] private TMP_Text boxTime;
    [SerializeField] private Image boxImage;
    [SerializeField] private Image boxBackgroundImage;
    [SerializeField] private Image boxSelectedImage;

    // 빈칸일 경우, boxName, boxImage, boxTime, boxBackground 모두 setactive가 false가 되어야 함.
    public void Init(int slotIndex, BoxDataSO boxdata)
    {
        SlotIndex = slotIndex;
        
        if(boxdata != null)
        {
            PutInSlot(boxdata);
        }
        else
        {
            EmptySlot();
        }
    }

    private void Update()
    {
        UpdateButtonActive();
    }


    public void PutInSlot(BoxDataSO data)
    {
        if(boxData != null) 
        {
            Logger.Log($"{SlotIndex}번째 index에 box가 있는데 넣으려 합니다.");
            return;
        }
        boxData = data;
        UpdateSlotVisual();
    }

    public void SlotSelected(bool isSelected)
    {
        boxSelectedImage.gameObject.SetActive(isSelected);
        assignButton.interactable = isSelected;
    }

    public void EmptySlot()
    {
        boxData = null;
        UpdateSlotVisual();
    }

    private void UpdateSlotVisual()
    {
        if(boxData != null)
        {
            boxName.text = boxData.name;
            boxImage.sprite = boxData.icon;
            TimeSpan time = TimeSpan.FromSeconds(boxData.unlockDurationSeconds);
            boxTime.text =
                string.Format("{0:D2}:{1:D2}:{2:D2}", (int)time.TotalHours, time.Minutes, time.Seconds);
        }
        else
        {
            boxName.text = string.Empty;
            boxTime.text = string.Empty;
            boxImage.gameObject.SetActive(false);
            boxBackgroundImage.gameObject.SetActive(false);
            assignButton.gameObject.SetActive(false);
        }
        
    }

    private void UpdateButtonActive()
    {
        if (BoxSlotManager.Instance.IsSlotFull())
        {
            assignButton.interactable = false;
        }
    }



}
