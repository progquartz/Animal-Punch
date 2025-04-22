using JetBrains.Annotations;
using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public PlayerStat InitialStat;
    public PlayerStat Stat;
    public Transform PlayerTransform;
    public Inventory Inventory;
    [SerializeField] private PlayerPhysics playerPhysics;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        base.Init();
        playerPhysics = GetComponent<PlayerPhysics>();
        Inventory = GetComponent<Inventory>();
        playerPhysics.Init(this);
        Stat.Init();
        InitialStat.Init();
        InitialStat.CopyData(Stat);
    }

    

}
