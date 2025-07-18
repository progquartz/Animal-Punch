using TMPro;
using UnityEngine;

public class ChestLootUnlockUI : BaseUI
{
    public PlayerModelPlaceHolder modelPlaceHolder;
    public TMP_Text modelName;
    public void OpenLoot(string modelKey)
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if (title != null)
        {
            title.OnAdditionalUIToggled(true);
        }
        if(modelPlaceHolder == null)
        {
            modelPlaceHolder = GetComponentInChildren<PlayerModelPlaceHolder>();
        }
        modelPlaceHolder.ToggleModel(true);
        modelPlaceHolder.ChangeModelKey(modelKey);
        modelName.text = modelKey;
    }

    public void CloseLoot()
    {
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        if (title != null)
        {
            title.OnAdditionalUIToggled(false);
        }
        else
        {
            Debug.Log("???");
        }
        Close();
    }
}
