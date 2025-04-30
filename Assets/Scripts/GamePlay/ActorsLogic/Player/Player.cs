using JetBrains.Annotations;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public PlayerStat InitialStat;
    public PlayerStat Stat;
    public Transform PlayerTransform;
    public Inventory Inventory;
    [SerializeField] private PlayerPhysics playerPhysics;
    [SerializeField] private AnimalAnimationController animationController;
    [SerializeField] private PlayerParticleController particleController;

    protected override void Awake()
    {
        IsDestroyOnLoad = true;
        base.Awake();
        Init();
    }

    public void StartGameState()
    {
        Init();
    }

    protected void Init()
    {
        IsDestroyOnLoad = true;

        base.Init();
        playerPhysics = GetComponent<PlayerPhysics>();
        Inventory = GetComponent<Inventory>();
        animationController = GetComponent<AnimalAnimationController>();
        particleController = GetComponent<PlayerParticleController>();
        playerPhysics.Init(this);
        Stat.Init();
        InitialStat.Init();
        InitialStat.CopyData(Stat);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
        GameManager.Instance.OnTimeToggle += particleController.OnTimeToggle;
        GameManager.Instance.OnQuitGameScene += ReleaseEvents;
        
    }

    private void ReleaseEvents()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
        GameManager.Instance.OnTimeToggle -= particleController.OnTimeToggle;
        GameManager.Instance.OnQuitGameScene -= ReleaseEvents;
        
    }

    private void OnTimeToggle(bool isTimeStop)
    {
        if (isTimeStop)
        {
            animationController.PauseAnimation();
        }
        else
        {
            animationController.ResumeAnimation();
        }
    }

    

}
