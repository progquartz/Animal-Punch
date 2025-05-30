using UnityEngine;

public class SceneChangerUI : MonoBehaviour
{
    public void ChangeScene(SceneType sceneType)
    {
        SceneLoader.Instance.LoadScene(sceneType);
    }

    public void ChangeSceneToGameScene()
    {
        ChangeScene(SceneType.GameScene);
    }

    public void ChangeSceneToTitleScene()
    {
        ChangeScene(SceneType.TitleScene);
    }
}
