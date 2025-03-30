using UnityEngine;

public class InteractableActor : MonoBehaviour
{
    public bool IsObjectHasHealth = false;
    public ActorsStat stat;


    private Rigidbody rb;
    private Collider objectCollider;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    public void HandleCollision(GameObject collisionObject, float impulseMagnitde)
    {
        if (collisionObject.CompareTag("Player"))
        {
            if (IsObjectHasHealth)
            {
                HandleCollisionOnHealthCondition(collisionObject, impulseMagnitde);
            }
            else
            {
                HandleCollisionOnNoneHealthCondition(collisionObject, impulseMagnitde);
            }
        }
    }

    private void HandleCollisionOnNoneHealthCondition(GameObject collision, float impulseMagnitude)
    {
        // Rigidbody�� ��� Constraints ����
        rb.constraints = RigidbodyConstraints.None;

        // Collider ��Ȱ��ȭ
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    private void HandleCollisionOnHealthCondition(GameObject collision, float impulseMagnitude)
    {
        // �÷��̾�Լ� �ӵ� �޾ƿ���

        // �÷��̾� �ӵ��� �°�


    }
}