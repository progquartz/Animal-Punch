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
        string sceneName = "Scenes/" + sceneType;

        if (CurrentScene == sceneName)
        {
            Logger.LogWarning("현재 있는 씬과 이동하려는 씬이 같아서 이동하지 않습니다.");
            return;
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

        // 씬 활성화가 완료된 다음 프레임까지 대기
        await Task.Yield();

        if (sceneType == SceneType.GameScene)
        {
            // MapManager가 생성될 때까지 대기
            while (MapManager.Instance == null)
            {
                await Task.Delay(100);
            }

            // EnemySpawner가 할당될 때까지 대기
            while (MapManager.Instance.EnemySpawner == null)
            {
                await Task.Delay(100);
            }

            // 풀링이 완료될 때까지 대기
            while (!MapManager.Instance.EnemySpawner.IsPoolingReady)
            {
                await Task.Delay(100);
            }

            GameManager.Instance.StartGameState();
        }

        _loaderCanvas.SetActive(false);
        CurrentScene = sceneName;
    }
}
