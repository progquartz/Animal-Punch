using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool IsObjectHasHealth = false;


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
            if (!IsObjectHasHealth)
            {
                HandleCollisionOnNoneHealthCondition(collisionObject, impulseMagnitde);
            }
            else
            {
                HandleCollisionOnHealthCondition(collisionObject, impulseMagnitde);
            }
        }
    }

    private void HandleCollisionOnNoneHealthCondition(GameObject collision, float impulseMagnitude)
    {
        // Rigidbody의 모든 Constraints 해제
        rb.constraints = RigidbodyConstraints.None;

        // Collider 비활성화
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    private void HandleCollisionOnHealthCondition(GameObject collision, float impulseMagnitude)
    {
        // 플레이어에게서 속도 받아오고

        // 플레이어 속도에 맞게


    }
}