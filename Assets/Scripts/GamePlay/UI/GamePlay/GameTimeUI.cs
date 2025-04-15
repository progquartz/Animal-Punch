using UnityEngine;
using UnityEngine.UI;

public class GameTimeUI : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Image gameTimeUIImage;
    private Vector3 originalUISize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        InitImage();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeUI();
    }

    private void InitImage()
    {
        originalUISize = gameTimeUIImage.transform.localScale;
    }

    private void UpdateTimeUI()
    {
        float GameTimeRatio = gameManager.GameTimeLeft / gameManager.MaxGameTime;
        gameTimeUIImage.transform.localScale = new Vector3(originalUISize.x * GameTimeRatio, originalUISize.y, originalUISize.z);

    }
}
