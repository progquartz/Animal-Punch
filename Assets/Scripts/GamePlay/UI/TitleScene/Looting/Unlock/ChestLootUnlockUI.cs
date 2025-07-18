using TMPro;
using UnityEngine;

public class ChestLootUnlockUI : BaseUI
{
    public PlayerModelPlaceHolder modelPlaceHolder;
    public TMP_Text modelName;
    public void OpenLoot(string modelKey)
    {
        modelPlaceHolder.ToggleModel(true);
    }
}
