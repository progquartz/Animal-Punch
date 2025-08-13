using UnityEngine;

public class TitleSceneInitiater : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.OpenUI<TitleUI>(new BaseUIData());
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        title.modelPlaceHolder = GetComponentInChildren<PlayerModelPlaceHolder>();
        title.OnAdditionalUIToggled(false);
        if (GameManager.Instance.isFirstTimeInTitle)
        {
            UIManager.Instance.OpenUI<TitleBackgroundUI>(new BaseUIData());
            GameManager.Instance.isFirstTimeInTitle = false;
            title.OnAdditionalUIToggled(true); // 모델 보이지 않게 넣은 것.
        }
    }

}
