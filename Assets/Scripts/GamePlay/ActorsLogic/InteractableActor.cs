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

    public void HandleCollision(GameObject collisionObject, float impulseDamage)
    {
        if (collisionObject.CompareTag("Player"))
        {
            if (IsObjectHasHealth)
            {
                HandleCollisionOnHealthCondition(collisionObject, impulseDamage);
            }
            else
            {
                HandleCollisionOnNoneHealthCondition(collisionObject, impulseDamage);
            }
        }
    }

    private void HandleCollisionOnNoneHealthCondition(GameObject collision, float impulseDamage)
    {
        // Rigidbody�� ��� Constraints ����
        rb.constraints = RigidbodyConstraints.None;

        // Collider ��Ȱ��ȭ
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    private void HandleCollisionOnHealthCondition(GameObject collision, float impulseDamage)
    {
        // �÷��̾�Լ� �ӵ� �޾ƿ���

        // �÷��̾� �ӵ��� �°�


    }
}