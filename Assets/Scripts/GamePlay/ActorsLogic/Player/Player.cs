using JetBrains.Annotations;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public PlayerStat InitialStat;
    public PlayerStat Stat;
    public Transform PlayerTransform;
    public Inventory Inventory;
    [SerializeField] private PlayerPhysics playerPhysics;
    public PlayerAnimationController animationController;
    public PlayerParticleController particleController;
    public PlayerComboHandler comboHandler;
    public PlayerFeverHandler feverHandler;

    protected override void Init()
    {
        IsDestroyOnLoad = true;

        base.Init();
        playerPhysics = GetComponent<PlayerPhysics>();
        Inventory = GetComponent<Inventory>();
        animationController = GetComponent<PlayerAnimationController>();
        particleController = GetComponent<PlayerParticleController>();
        playerPhysics.Init(this);
        animationController.Init(this);
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

    public bool GainExp(int amount)
    {
        bool isLevelUp = Stat.GainExp(amount);
        feverHandler.AddFeverGauge(amount);
        return isLevelUp;
    }

    public void OnLevelUp(bool isSelectedLooting)
    {
        if(isSelectedLooting)
        {
            particleController.OnLevelUp();
        }
    }

    

}
