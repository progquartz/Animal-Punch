using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class WanderPattern : IActorPattern
{
    private float rotationSpeed = 500f;

    private EnemyMoving owner;

    private Quaternion wanderRotation;
    private float changeDirectionTime;
    private bool isRotating = false;

    public void ActPattern()
    {
        MoveFront();
        HandleRotation();
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        owner.animationController.ChangeAnimation(AnimalAnimation.Walk);
    }

    private void MoveFront()
    {
        // ���� ������Ʈ�� ���� �������� �̵�
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
            // angle ����� �ƴ� quaternion �� vector ����� ������, ������ �� �������θ� ȸ���� ���ɼ��� ���ܼ�.
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
            // ���� ȸ���� ��ǥ ȸ���� ���̰� ����� ������ ȸ�� �Ϸ�� ����
            if (Quaternion.Angle(owner.EnemyTransform.rotation, wanderRotation) < 0.1f)
            {
                isRotating = false;
                changeDirectionTime = Random.Range(2f, 5f); // ���� ȸ�������� ��� �ð� ����
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

    public void Init(EnemyMoving enemy)
    {
        owner = enemy;
    }
}
