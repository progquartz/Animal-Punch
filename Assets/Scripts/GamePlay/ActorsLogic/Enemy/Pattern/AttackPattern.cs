using UnityEngine;

public class AttackPattern : IActorPattern
{
    private float rotationSpeed = 400f;

    private Enemy owner;

    private Quaternion fleeRotation;
    private bool isRotating = false;
    private float changeDirectionTime;


    public void ActPattern()
    {
        MoveFront(); // 사실 도망치기이긴 한데
        HandleRotation();
        HandleAnimation();
    }

    private void MoveFront()
    {
        // 현재 오브젝트의 앞쪽 방향으로 이동
        Vector3 movement = owner.EnemyTransform.forward * owner.stat.Speed * Time.deltaTime;
        owner.EnemyRB.MovePosition(owner.EnemyRB.position + movement);
    }

    private void HandleAnimation()
    {
        //owner.animationController.ChangeAnimation(AnimalAnimation.Walk);
    }

    private void HandleRotation()
    {
        if (changeDirectionTime > 0f)
        {
            changeDirectionTime -= Time.deltaTime;
        }
        else if (!isRotating)
        {
            // angle 기반이 아닌 quaternion 과 vector 기반인 이유는, 무조건 한 방향으로만 회전할 가능성이 생겨서.
            Vector3 oppositeDirection = Player.Instance.PlayerTransform.position - owner.transform.position;
            oppositeDirection.Normalize();

            fleeRotation = Quaternion.LookRotation(oppositeDirection);
            isRotating = true;
        }

        if (isRotating)
        {
            owner.EnemyTransform.rotation = Quaternion.RotateTowards(
                owner.EnemyTransform.rotation,
                fleeRotation,
                rotationSpeed * Time.deltaTime
            );
            // 현재 회전과 목표 회전의 차이가 충분히 작으면 회전 완료로 간주
            if (Quaternion.Angle(owner.EnemyTransform.rotation, fleeRotation) < 0.1f)
            {
                isRotating = false;
                changeDirectionTime = Random.Range(1f, 2f); // 다음 회전까지의 대기 시간 설정
            }
        }

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
