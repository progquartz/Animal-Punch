using UnityEngine;

public class TitleSceneInitiater : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"TitleSceneInitialize");
        UIManager.Instance.OpenUI<TitleUI>(new BaseUIData());
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        title.modelPlaceHolder = GetComponentInChildren<PlayerModelPlaceHolder>();
        title.OnAdditionalUIToggled(false);
        Debug.Log($"1");
        if (GameManager.Instance.isFirstTimeInTitle)
        {
            Debug.Log($"2");
            UIManager.Instance.OpenUI<TitleBackgroundUI>(new BaseUIData());
            GameManager.Instance.isFirstTimeInTitle = false;
            title.OnAdditionalUIToggled(true); // 모델 보이지 않게 넣은 것.
        }
    }

}
