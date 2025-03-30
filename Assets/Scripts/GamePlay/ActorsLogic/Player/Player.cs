using UnityEngine;

public class Player : SingletonBehaviour<Player>
{
    public PlayerStat stat;

    [SerializeField] private PlayerPhysics playerPhysics;


    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        base.Init();
        playerPhysics = GetComponent<PlayerPhysics>();
        playerPhysics.Init(this);
    }




    

}
