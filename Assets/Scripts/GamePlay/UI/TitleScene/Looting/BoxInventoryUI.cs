using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoxInventoryUI : MonoBehaviour
{
    public GameObject boxItemPrefab;             // UI 프리팹 (버튼 + 이미지 + 텍스트)
    public Transform contentParent;              // LayoutGroup 안에 배치될 부모
    public List<BoxDataSO> allBoxDataList;         // 등록된 모든 BoxData 목록

    public Button assignButton;                  // "슬롯에 넣기" 버튼
    private string selectedBoxId = null;

    public int selectedSlotIndex = 0;            // 슬롯 인덱스 (외부에서 지정 필요)

    void OnEnable()
    {
        PopulateUI();
    }

    void PopulateUI()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var owned in BoxInventoryManager.Instance.GetOwnedBoxes())
        {
            BoxDataSO boxData = allBoxDataList.Find(b => b.id == owned.boxId);
            if (boxData == null) continue;

            GameObject go = Instantiate(boxItemPrefab, contentParent);
            go.transform.Find("Name").GetComponent<Text>().text = boxData.displayName;
            go.transform.Find("Icon").GetComponent<Image>().sprite = boxData.icon;
            go.transform.Find("Time").GetComponent<Text>().text = $"{boxData.unlockDurationSeconds / 60}분";
            go.transform.Find("Count").GetComponent<Text>().text = $"x{owned.count}";

            Button button = go.GetComponent<Button>();
            string boxId = boxData.id;
            button.onClick.AddListener(() =>
            {
                selectedBoxId = boxId;
                assignButton.interactable = true;
            });
        }

        assignButton.interactable = false;
    }

    public void OnAssignButtonPressed()
    {
        if (!string.IsNullOrEmpty(selectedBoxId))
        {
            if (BoxInventoryManager.Instance.UseBox(selectedBoxId))
            {
                
                BoxDataSO boxData = allBoxDataList.Find(b => b.id == selectedBoxId);
                LootingBoxSlots boxSlotManager = FindObjectOfType<LootingBoxSlots>();
                boxSlotManager.AssignBoxToSlot(selectedSlotIndex, boxData);
                gameObject.SetActive(false);
            }
        }
    }
}
