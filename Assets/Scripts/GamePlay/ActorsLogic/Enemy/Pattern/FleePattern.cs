using UnityEngine;

public class FleePattern : IActorPattern
{
    private Enemy owner;


    public void ActPattern()
    {
        Transform playerPos = Player.Instance.transform;
        // �ݴ� �������� ��������!
    }


    public void Init(Enemy enemy)
    {
        owner = enemy;
    }
}
