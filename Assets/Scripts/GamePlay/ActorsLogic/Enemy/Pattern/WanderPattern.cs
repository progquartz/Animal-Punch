using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class WanderPattern : IActorPattern
{
    private float DetectionRange = 2f;
    private float rotationSpeed = 500f;

    private Enemy owner;

    private Quaternion wanderRotation;
    private float changeDirectionTime;
    private bool isRotating = false;

    public void ActPattern()
    {
        MoveFront();
        HandleRotation();
    }

    private void MoveFront()
    {
        // 현재 오브젝트의 앞쪽 방향으로 이동
        Vector3 movement = owner.EnemyTransform.forward * owner.stat.Speed * Time.deltaTime;
        owner.EnemyRB.MovePosition(owner.EnemyRB.position + movement);
    }

    private void HandleRotation()
    {
        if (changeDirectionTime > 0f)
        {
            changeDirectionTime -= Time.deltaTime;
        }
        else if (!isRotating)
        {
            wanderRotation = Quaternion.LookRotation(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized);
            isRotating = true;
        }

        if (isRotating)
        {
            owner.EnemyTransform.rotation = Quaternion.RotateTowards(
                owner.EnemyTransform.rotation,
                wanderRotation,
                rotationSpeed * Time.deltaTime
            );
            // 현재 회전과 목표 회전의 차이가 충분히 작으면 회전 완료로 간주
            if (Quaternion.Angle(owner.EnemyTransform.rotation, wanderRotation) < 0.1f)
            {
                isRotating = false;
                changeDirectionTime = Random.Range(2f, 5f); // 다음 회전까지의 대기 시간 설정
            }
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
