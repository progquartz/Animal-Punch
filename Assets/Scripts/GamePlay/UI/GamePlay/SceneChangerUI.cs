using UnityEngine;

public class SceneChangerUI : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }
}
