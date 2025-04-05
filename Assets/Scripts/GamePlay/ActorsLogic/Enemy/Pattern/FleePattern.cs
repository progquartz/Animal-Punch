using UnityEngine;

public class FleePattern : IActorPattern
{
    private float DetectionRange = 10f;

    private Enemy owner;

    private Vector3 fleeDirection;
    private float changeDirectionTime;

    public void ActPattern()
    {
        Transform playerPos = Player.Instance.transform;

        // 무작위 방향 설정
        fleeDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        // 일정 시간 후 방향 전환하도록 타이머 설정
        changeDirectionTime = Random.Range(2f, 5f);
        // 걷기 애니메이션 재생 (달리기 해제)

        //owner.animationController.ChangeAnimation(AnimalAnimation.Walk);

        // 반대 방향으로 도망가기!
        // 플레이어 감지 시 상태 전환
        float dist = Vector3.Distance(owner.transform.position, owner. transform.position);
        if (dist < DetectionRange)
        {
            //owner.rigidbody.linearVelocity = wanderDirection * owner.stats.MoveSpeed;
            // 타이머 경과 체크하여 방향 변경
            changeDirectionTime -= Time.deltaTime;
            if (changeDirectionTime <= 0f)
            {
                fleeDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                changeDirectionTime = Random.Range(2f, 5f);
            }
        }
        else
        {
            changeDirectionTime = 0f;
        }

        // 지속적으로 배회 방향으로 이동
        

    }

    public void EnterPattern()
    {
        Logger.Log("FleePattern Enter");
    }

    public void ExitPattern()
    {
        Logger.Log("FleePattern Exit");
    }

    public void Init(Enemy enemy)
    {
        owner = enemy;
    }
}
