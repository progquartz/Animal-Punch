using UnityEngine;

public class TitleSceneInitiater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.OpenUI<TitleBackgroundUI>(new BaseUIData());
    }
}
