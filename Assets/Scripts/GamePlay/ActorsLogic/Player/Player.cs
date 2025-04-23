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

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        base.Init();
        playerPhysics = GetComponent<PlayerPhysics>();
        Inventory = GetComponent<Inventory>();
        animationController = GetComponent<AnimalAnimationController>();
        playerPhysics.Init(this);
        Stat.Init();
        InitialStat.Init();
        InitialStat.CopyData(Stat);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
    }

    private void ReleaseEvents()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
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
