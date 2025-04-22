using UnityEngine;

public class LootingManager : SingletonBehaviour<LootingManager>
{
    public GameObject LootingPrefab;
    public GameObject LootingParent;

    [SerializeField] private DropItemData lootIncludeingItem;
    [SerializeField] private DropItemData lootExcludingItem;

    /// <summary>
    /// 해당 월드포지션에 경험치와골드를 주는 드랍을 드랍.
    /// </summary>
    public void DropLoot(bool isIncludingItem, Vector3 worldPosition)
    {
        GameObject instance = Instantiate(LootingPrefab, worldPosition, Quaternion.identity, LootingParent.transform);
        DropLoot loot = instance.GetComponent<DropLoot>();
        if(isIncludingItem)
        {
            loot.Init(lootIncludeingItem);
        }
        else
        {
            loot.Init(lootExcludingItem);
        }
        
    }

    public void OpenLootUI()
    {
        UIManager.Instance.OpenUI<LootingUI>(new BaseUIData());
    }
}
