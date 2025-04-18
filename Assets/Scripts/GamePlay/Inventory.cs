using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int gold;


    public void GainGold(int amount)
    {
        gold += amount;
    }
}
