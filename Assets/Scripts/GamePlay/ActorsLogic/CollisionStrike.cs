using UnityEngine;

public class CollisionStrike : MonoBehaviour
{
    private Rigidbody rb;
    private Collider objectCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Rigidbody의 모든 Constraints 해제
            rb.constraints = RigidbodyConstraints.None;

            // Collider 비활성화
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }

            // 충돌 충격량은 별도의 처리 없이 유지됨 (기본 물리로 처리)
        }
    }
}