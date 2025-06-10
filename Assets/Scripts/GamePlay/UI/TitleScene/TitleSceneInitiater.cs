using UnityEngine;

public class TitleSceneInitiater : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.OpenUI<TitleUI>(new BaseUIData());
        TitleUI title = UIManager.Instance.GetActiveUI<TitleUI>() as TitleUI;
        title.IsAdditionalUIOpened = true; // 모델 보이지 않게 넣은 것.

        UIManager.Instance.OpenUI<TitleBackgroundUI>(new BaseUIData());
    }

}
