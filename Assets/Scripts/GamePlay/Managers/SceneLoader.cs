using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    GameScene,
    TitleScene
}

public class SceneLoader : SingletonBehaviour<SceneLoader> 
{
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    public string CurrentScene;

    protected override void Init()
    {
        base.Init();
        CurrentScene = "Scenes/" + SceneManager.GetActiveScene().name;
    }

    public async void LoadScene(SceneType sceneType)
    {
        string sceneName = "Scenes/" + sceneType.ToString();

        if(CurrentScene == sceneName)
        {
            Logger.LogWarning("현재 있는 씬과 이동하려는 씬이 같아서 이동하지 않습니다.");
            return; 
        }

        if (CurrentScene == "Scenes/GameScene")
        {
            GameManager.Instance.EndGameState();
        }

        Logger.Log($"{sceneName}으로 씬을 이동시킵니다.");
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _progressBar.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        CurrentScene = sceneName;
        if(sceneName == "Scenes/GameScene")
        {
            GameManager.Instance.StartGameState();
        }
        _loaderCanvas.SetActive(false);

    }
}
