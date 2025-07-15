using JetBrains.Annotations;
using System;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public PlayerStat InitialStat;
    public PlayerStat Stat;
    public Transform PlayerTransform;
    [SerializeField] private PlayerPhysics playerPhysics;
    public PlayerAnimationController animationController;
    public PlayerParticleController particleController;
    public PlayerComboHandler comboHandler;
    public PlayerFeverHandler feverHandler;

    public Action OnDash;
    public Action OnFeverStart;
    public Action OnFeverEnd;
    public Action OnLevelUp;


    protected override void Init()
    {
        IsDestroyOnLoad = true;

        base.Init();
        playerPhysics = GetComponent<PlayerPhysics>();
        animationController = GetComponent<PlayerAnimationController>();
        particleController = GetComponent<PlayerParticleController>();
        playerPhysics.Init(this);
        animationController.Init(this);
        Stat.Init();
        InitialStat.Init();
        Stat.CopyData(InitialStat);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
        GameManager.Instance.OnTimeToggle += particleController.OnTimeToggle;
        GameManager.Instance.OnQuitGameScene += ReleaseEvents;
        OnDash += playerPhysics.Dash;
        
    }

    private void ReleaseEvents()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
        GameManager.Instance.OnTimeToggle -= particleController.OnTimeToggle;
        GameManager.Instance.OnQuitGameScene -= ReleaseEvents;
        OnDash -= playerPhysics.Dash;

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
        // 피버에는 exp 배율 적용하면 안됨.
        feverHandler.AddFeverGauge(amount);

        // exp 배율 적용 필요
        bool isLevelUp = Stat.GainExp(amount);
        
        return isLevelUp;
    }
}
