using UnityEngine;

public class LootingManager : SingletonBehaviour<LootingManager>
{
    public GameObject LootingPrefab;
    public GameObject LootingParent;

    [SerializeField] private DropItemData lootIncludeingItem;
    [SerializeField] private DropItemData lootExcludingItem;

    protected override void Init()
    {
        IsDestroyOnLoad = true;
        ChangeParentToManagers();
        base.Init();

    }
    /// <summary>
    /// 해당 월드포지션에 경험치와골드를 주는 드랍을 드랍.
    /// </summary>
    public void DropLoot(DropItemData dropItemData, Vector3 worldPosition)
    {
        if(LootingParent == null)
        {
            LootingParent = new GameObject("LootingParent");
            LootingParent.transform.parent = null;
        }
        GameObject instance = Instantiate(LootingPrefab, worldPosition, Quaternion.identity, LootingParent.transform);
        DropLoot loot = instance.GetComponent<DropLoot>();
        loot.Init(dropItemData);
    }

    public void OpenLootUI()
    {
        GameManager.Instance.StopTime();
        UIManager.Instance.OpenUI<LootingUI>(new BaseUIData());
    }
}
