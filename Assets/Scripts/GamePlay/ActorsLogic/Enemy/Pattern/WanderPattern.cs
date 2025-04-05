using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class WanderPattern : IActorPattern
{
    private float DetectionRange = 10f;

    private Enemy owner;

    private Vector3 wanderDirection;
    private float changeDirectionTime;

    public void ActPattern()
    {
        Transform playerPos = Player.Instance.transform;

        changeDirectionTime -= Time.deltaTime;

        if (changeDirectionTime<= 0f)
        {
            // 무작위 방향 설정
            //owner.animationController.ChangeAnimation(AnimalAnimation.Walk);
            wanderDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            changeDirectionTime = Random.Range(2f, 5f);
        }
    }

    public void EnterPattern()
    {
        Logger.Log("WanderPattern Enter");
    }

    public void ExitPattern()
    {
        Logger.Log("WanderPattern Exit");
    }

    public void Init(Enemy enemy)
    {
        owner = enemy;
    }
}
