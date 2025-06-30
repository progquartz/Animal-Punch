using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold;
    public int gem;

    public void GainGold(int amount)
    {
        gold += amount;
    }
}
