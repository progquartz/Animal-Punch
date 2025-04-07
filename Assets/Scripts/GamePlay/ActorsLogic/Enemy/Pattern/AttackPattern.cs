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
        MoveFront(); // ��� ����ġ���̱� �ѵ�
        HandleRotation();
        HandleAnimation();
    }

    private void MoveFront()
    {
        // ���� ������Ʈ�� ���� �������� �̵�
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
            // angle ����� �ƴ� quaternion �� vector ����� ������, ������ �� �������θ� ȸ���� ���ɼ��� ���ܼ�.
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
            // ���� ȸ���� ��ǥ ȸ���� ���̰� ����� ������ ȸ�� �Ϸ�� ����
            if (Quaternion.Angle(owner.EnemyTransform.rotation, fleeRotation) < 0.1f)
            {
                isRotating = false;
                changeDirectionTime = Random.Range(1f, 2f); // ���� ȸ�������� ��� �ð� ����
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
