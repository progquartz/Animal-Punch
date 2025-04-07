using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerPhysics owner;

    private Transform playerTransform;
    private Rigidbody playerRB;
    

    // �浹 �� �ӵ��� ������ ����
    private Vector3 previousVelocity;

    public void Init(PlayerPhysics owner)
    {
        this.owner = owner;
        playerRB = owner.playerRB;
        playerTransform = owner.playerTransform;
    }

    private void FixedUpdate()
    {
        previousVelocity = playerRB.linearVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Interactable"))
        {
            ActorCollision interactable = collision.gameObject.GetComponent<ActorCollision>();
            if(interactable != null)
            {
                // �浹�� ��� �ӵ��� �پ��� ������, �̸� ������� �پ�� �ӵ� = ��ݷ����� ������ ����� ����.
                // �ܼ� ��ݷ��� ���� �ʴ� ������, �̸� �޾Ƴ��� ��ü�� ���� ������ �ؾ� �ϱ� ����.
                Vector3 velocityChanged = playerRB.linearVelocity - previousVelocity;
                float impulseMagnitude = velocityChanged.magnitude;

                //Debug.Log($"����� �Ͼ����, �� ��ݷ��� {impulseMagnitude}�Դϴ�.");
                float impulseDamage = owner.CalculateImpulseDamage(impulseMagnitude);
                interactable.HandleCollision(collision, playerTransform.gameObject, impulseDamage);
            }
        }
    }
}
