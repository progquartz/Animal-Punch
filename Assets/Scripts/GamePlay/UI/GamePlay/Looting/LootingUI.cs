using UnityEngine;

public enum LootingRankType
{
    Normal = 0,
    Rare = 1,
    Unique = 2,
    Epic = 3,
    Legendary = 4,
}

public enum LootingTypeType
{

}
public class LootingUI : BaseUI
{
    [SerializeField] public Color[] RankColors;

    [SerializeField] private LootingCardUI[] lootingCardUIList;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
