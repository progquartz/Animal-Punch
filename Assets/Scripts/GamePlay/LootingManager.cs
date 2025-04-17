using UnityEngine;

public class LootingManager : SingletonBehaviour<LootingManager>
{
    public GameObject LootingPrefab;
    public GameObject LootingParent;

    [SerializeField] private DropItemData lootIncludeingItem;
    [SerializeField] private DropItemData lootExcludingItem;
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
}
