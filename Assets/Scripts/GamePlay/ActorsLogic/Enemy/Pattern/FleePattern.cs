using UnityEngine;

public class FleePattern : IActorPattern
{
    private Enemy owner;


    public void ActPattern()
    {
        Transform playerPos = Player.Instance.transform;
        // 반대 방향으로 도망가기!
    }


    public void Init(Enemy enemy)
    {
        owner = enemy;
    }
}
