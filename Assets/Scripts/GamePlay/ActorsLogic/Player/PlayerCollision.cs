using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody rb;
    private PlayerPhysics playerMove;

    // �浹 �� �ӵ��� ������ ����
    private Vector3 previousVelocity;

    private void Start()
    {
        playerMove = playerTransform.GetComponent<PlayerPhysics>();
        rb = playerTransform.GetComponent<Rigidbody>(); 

    }

    private void FixedUpdate()
    {
        previousVelocity = rb.linearVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Interactable"))
        {
            InteractableActor interactable = collision.gameObject.GetComponent<InteractableActor>();
            if(interactable != null)
            {
                // �浹�� ��� �ӵ��� �پ��� ������, �̸� ������� �پ�� �ӵ� = ��ݷ����� ������ ����� ����.
                // �ܼ� ��ݷ��� ���� �ʴ� ������, �̸� �޾Ƴ��� ��ü�� ���� ������ �ؾ� �ϱ� ����.
                Vector3 velocityChanged = rb.linearVelocity - previousVelocity;
                float impulseMagnitude = velocityChanged.magnitude;

                Debug.Log($"����� �Ͼ����, �� ��ݷ��� {impulseMagnitude}�Դϴ�.");
                interactable.HandleCollision(playerTransform.gameObject, impulseMagnitude);
            }
        }
    }
}
