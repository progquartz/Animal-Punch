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
        // Rigidbody의 모든 Constraints 해제
        rb.constraints = RigidbodyConstraints.None;

        // Collider 비활성화
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    private void HandleCollisionOnHealthCondition(GameObject collision, float impulseDamage)
    {
        // 플레이어에게서 속도 받아오고

        // 플레이어 속도에 맞게


    }
}