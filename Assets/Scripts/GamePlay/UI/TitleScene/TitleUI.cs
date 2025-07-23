using UnityEngine;

public class TitleUI : BaseUI
{
    public bool IsAdditionalUIOpened = false;
    public PlayerModelPlaceHolder modelPlaceHolder;

    private void Awake()
    {
        modelPlaceHolder.ChangeModelKey(UnlockSaveManager.Instance.selectedIds);
    }

    public void OpenUnlockUI()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        UIManager.Instance.OpenUI<UnLockUI>(new BaseUIData());
        OnAdditionalUIToggled(true);
    }
    public void OpenInventory()
    {
        SoundManager.Instance.PlaySFX("ButtonClick", AudioType.UI);
        UIManager.Instance.OpenUI<BoxInventoryUI>(new BaseUIData());
        OnAdditionalUIToggled(true);
    }

    public void OpenSettingUI()
    {
        UIManager.Instance.OpenUI<SettingsUI>(new BaseUIData());
        OnAdditionalUIToggled(true);
    }

    public void OnClickPlayButton()
    {
        SceneLoader.Instance.LoadScene(SceneType.GameScene);
    }

    public void OnAdditionalUIToggled(bool isAdditionalUIOpened)
    {
        this.IsAdditionalUIOpened = isAdditionalUIOpened;
        if(modelPlaceHolder == null)
        {
            modelPlaceHolder = GetComponentInChildren<PlayerModelPlaceHolder>();
        }
        //Debug.Log($"Toggle Model {!isAdditionalUIOpened}");
        modelPlaceHolder.ToggleModel(!isAdditionalUIOpened);
    }

    public void OnSelectedCharacterChange(string id)
    {
        modelPlaceHolder.ChangeModelKey(id);
    }

}
