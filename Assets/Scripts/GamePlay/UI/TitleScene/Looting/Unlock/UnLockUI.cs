using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UnLockUI : BaseUI
{
    public Transform gridParent;

    public GameObject unlockableItemPrefab;
    public ScrollRect scrollRect;
    public List<UnLockSlotUI> slots;

    public override void Init(Transform canvas)
    {
        transform.SetParent(canvas);

        var rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        InitUI();
        RearrangePosition();
    }

    private void Update()
    {
        if(UnlockSaveManager.Instance.hasChange)
        {
            InitUI();
            UnlockSaveManager.Instance.hasChange = false;
        }
    }

    private void RearrangePosition()
    {
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public void InitUI()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var unlockable in UnlockSaveManager.Instance.allUnlocks)
        {
            UnlockSaveManager unlockManager = UnlockSaveManager.Instance;
            UnLockSlotUI slot = Instantiate(unlockableItemPrefab, gridParent).GetComponent<UnLockSlotUI>();
            bool isSlotUnlocked = unlockManager.IsUnlocked(unlockable.id);
            bool isSlotSelected = (unlockManager.selectedIds == unlockable.id);
            slot.ChangeUI(isSlotUnlocked , isSlotSelected , unlockable);

            // 선택하게 되면, selected를 바꾸게 하고, refresh하도록 설정.
            slot.SelectButton.onClick.AddListener(() =>
            {
                // 언락이면...
                if(UnlockSaveManager.Instance.IsUnlocked(slot.unlockableId))
                {
                    UnlockSaveManager.Instance.ChangeSelected(slot.unlockableId);
                }
                // 언락 안되었으면...
                else
                {
                    // 구매 UI로 처리해야 함.
                    UnlockSaveManager.Instance.HandleBuyItem(slot.unlockableId);
                }
            });
        }
    }

    public void OnClickCloseButton()
    {
        Close();
    }
}
