using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoxInventoryUI : BaseUI
{
    public GameObject boxItemPrefab;             // UI 프리팹 (버튼 + 이미지 + 텍스트)
    public Transform contentParent;              // LayoutGroup 안에 배치될 부모\
    public ScrollRect scrollRect;

    public int maxInventoryBoxCount = 21;
    public int selectedBoxIndex = 0;            // 슬롯 버튼을 클릭해서 선택되는 박스 인덱스

    public override void Init(Transform canvas)
    {
        //base.Init(canvas);

        transform.SetParent(canvas);

        var rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;

        InitUI();
        RearrangePosition();
    }

    private void RearrangePosition()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void InitUI()
    {
        List<BoxInventory.BoxCount> boxKeyList = InventoryManager.Instance.GetOwnedBoxes();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        for (i = 0; i < boxKeyList.Count; i++)
        {
            BoxDataSO boxData = DataManager.Instance.LootingStorage.AllBoxDataList.Find(b => b.rank == boxKeyList[i].boxId);
            if (boxData == null) continue;
            AssignNewSlot(i, boxData);
        }

        // 최대 칸 수 이하일 경우, 나머지 칸을 빈칸으로 설정.
        if(boxKeyList.Count < maxInventoryBoxCount)
        {
            // 빈 칸을 추가...
            for(; i < maxInventoryBoxCount; i++)
            {
                BoxDataSO boxData = null;
                AssignNewSlot(i, boxData);
            }
        }
        
    }

    private void AssignNewSlot(int index, BoxDataSO boxData)
    {
        GameObject slot = Instantiate(boxItemPrefab, contentParent);
        BoxInventorySlotUI slotUI = slot.GetComponent<BoxInventorySlotUI>();
        AssignSlotButton(slotUI);
        AssignAssignButton(slotUI);

        slotUI.Init(index, boxData);
        slotUI.SlotIndex = index;


    }



    private void AssignSlotButton(BoxInventorySlotUI slotUI)
    {
        Button slotButton = slotUI.slotButton;
        // 버튼 클릭시 listner 추가.
        slotButton.onClick.AddListener(() =>
        {
            // 기존에 선택했던 assignbutton을 비활성화하고...
            if (selectedBoxIndex != -1)
            {
                GetSlotInIndex(selectedBoxIndex).SlotSelected(false);
            }
            // 선택한 button active.
            selectedBoxIndex = slotUI.SlotIndex;
            if(GetSlotInIndex(selectedBoxIndex).boxData != null)
            {
                slotUI.SlotSelected(true);
            }
            
        });
    }

    private void AssignAssignButton(BoxInventorySlotUI slotUI)
    {
        Button assignButton = slotUI.assignButton;
        assignButton.onClick.AddListener(() =>
        {
            if (InventoryManager.Instance.AssignBox(selectedBoxIndex, BoxSlotManager.Instance.slotButtonRequestIndex))
            {
                EraseSlot(selectedBoxIndex);
                selectedBoxIndex = -1;
                OnClickCloseButton();
            }
        });
        assignButton.interactable = false;
    }

    private BoxInventorySlotUI GetSlotInIndex(int index)
    {
        return contentParent.GetChild(index).gameObject.GetComponent<BoxInventorySlotUI>();
    }

    private void EraseSlot(int index)
    {
        if(index == selectedBoxIndex)
        {
            selectedBoxIndex = -1;
        }
        GameObject slot = contentParent.GetChild(index).gameObject;
        Destroy(slot);
        List<BoxInventory.BoxCount> boxKeyList = InventoryManager.Instance.GetOwnedBoxes();
        if(boxKeyList.Count < maxInventoryBoxCount)
        {
            // 추가...
            for(int i = boxKeyList.Count; i < maxInventoryBoxCount; i++)
            {
                BoxDataSO boxData = null;
                AssignNewSlot(i, boxData);
            }
        }
    }

    public void OnClickCloseButton()
    {
        // request 취소.
        BoxSlotManager.Instance.slotButtonRequestIndex = -1;
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if(title != null)
        {
            title.IsAdditionalUIOpened = false;
        }
        Close();
    }
}
