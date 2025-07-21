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

    public Vector2 progressBarOriginalSize;
    public string CurrentScene;

    private float currentProgress = 0f;

    protected override void Init()
    {
        progressBarOriginalSize = _progressBar.rectTransform.sizeDelta;
        base.Init();
        CurrentScene = "Scenes/" + SceneManager.GetActiveScene().name;
    }

    public async void LoadScene(SceneType sceneType)
    {
        if (!CanLoadScene(sceneType)) return;

        await LoadSceneAsync(sceneType);
        if (sceneType == SceneType.GameScene)
        {
            await LoadPoolingDataAsync();
            GameManager.Instance.StartGameState();
        }

        FinishLoading(sceneType);
    }

    private bool CanLoadScene(SceneType sceneType)
    {
        string sceneName = "Scenes/" + sceneType;
        if (CurrentScene == sceneName)
        {
            Logger.LogWarning("현재 있는 씬과 이동하려는 씬이 같아서 이동하지 않습니다.");
            return false;
        }
        return true;
    }

    private async Task LoadSceneAsync(SceneType sceneType)
    {
        string sceneName = "Scenes/" + sceneType;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        while (scene.progress < 0.9f)
        {
            UpdateProgressBar(scene.progress / 0.9f * 0.5f);
            await Task.Delay(100);
        }

        scene.allowSceneActivation = true;
        await Task.Yield();

        CurrentScene = sceneName;
    }

    // 풀링 데이터 미리 로드하는 부분
    private async Task LoadPoolingDataAsync()
    {
        await WaitForEnemyPoolingAsync();
        await WaitForMapObjectPoolingAsync();
    }

    private async Task WaitForEnemyPoolingAsync()
    {
        while (MapManager.Instance == null ||
               MapManager.Instance.EnemySpawner == null ||
               !MapManager.Instance.EnemySpawner.IsPoolingReady)
        {
            IncrementProgressBar(0.5f, 0.75f);
            await Task.Delay(100);
        }
    }

    private async Task WaitForMapObjectPoolingAsync()
    {
        bool isMapManagerReady = false;
        void OnInitialized() => isMapManagerReady = true;
        MapManager.OnMapManagerInitialized += OnInitialized;

        while (!isMapManagerReady)
        {
            IncrementProgressBar(0.75f, 1f);
            await Task.Delay(100);
        }

        MapManager.OnMapManagerInitialized -= OnInitialized;
    }

    private async void FinishLoading(SceneType sceneType)
    {
        _progressBar.fillAmount = 1f;
        await Task.Delay(200);

        _loaderCanvas.SetActive(false);
        Logger.Log($"{sceneType} 로딩 완료");
    }

    private void UpdateProgressBar(float targetProgress)
    {
        currentProgress = targetProgress;
        _progressBar.rectTransform.sizeDelta = new Vector2(progressBarOriginalSize.x * currentProgress, progressBarOriginalSize.y);
    }

    private void IncrementProgressBar(float start, float end, float increment = 0.01f)
    {
        currentProgress = Mathf.Min(currentProgress + increment, end);
        _progressBar.rectTransform.sizeDelta = new Vector2(progressBarOriginalSize.x * currentProgress, progressBarOriginalSize.y);
    }
}
