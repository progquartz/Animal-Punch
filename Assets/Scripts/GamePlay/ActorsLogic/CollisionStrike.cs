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
            // Rigidbody�� ��� Constraints ����
            rb.constraints = RigidbodyConstraints.None;

            // Collider ��Ȱ��ȭ
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }

            // �浹 ��ݷ��� ������ ó�� ���� ������ (�⺻ ������ ó��)
        }
    }
}