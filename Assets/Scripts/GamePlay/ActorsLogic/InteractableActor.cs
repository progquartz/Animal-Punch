using UnityEditorInternal;
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

    public void HandleCollision(Collision collision, GameObject collisionObject, float impulseDamage)
    {
        if (collisionObject.CompareTag("Player"))
        {
            if (IsObjectHasHealth)
            {
                HandleCollisionOnHealthCondition(collision, collisionObject, impulseDamage);
            }
            else
            {
                HandleCollisionOnNoneHealthCondition(collision,collisionObject, impulseDamage);
            }
        }
    }

    private void HandleCollisionOnNoneHealthCondition(Collision collision, GameObject collisionObjects, float impulseDamage)
    {
        HandleDeadOnRigidBody();
    }



    private void HandleCollisionOnHealthCondition(Collision collision, GameObject collisionObject, float impulseDamage)
    {
        // 플레이어에게서 속도 받아오고

        // 플레이어 속도에 맞게
        HandleCollisionDamage(collision, collisionObject, impulseDamage);

    }

    private void HandleCollisionDamage(Collision collision, GameObject collisionObject, float impulseDamage)
    {
        bool isDead = stat.HandleDamage(impulseDamage);
        if(isDead)
        {
            HandleMassForceChange(collision);
            HandleShootingUp();
            HandleDeadOnRigidBody();
        }
        else
        {
            
        }
    }

    private void HandleMassForceChange(Collision collision)
    {
        // 충돌 시 발생한 impulse. 충돌한 객체로부터 받아낸 impulse이기에 -1을 곱해야 함.
        Vector3 impulse = collision.impulse / 30;

        // 현재 운동 상태 초기화
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 질량 변경
        rb.mass = stat.MassOnDead;
        rb.linearDamping = stat.DragOnDead;
        rb.angularDamping = stat.AngularDragOnDead;

        // 저장한 impulse를 다시 적용하여 충돌 효과 재현
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    private void HandleShootingUp()
    {
        rb.AddForce(new Vector3(0,10,0), ForceMode.Impulse);  
        
    }

    private void Spin()
    {
        rb.AddTorque(new Vector3(Random.value, Random.value, Random.value), ForceMode.Impulse);
    }

    private void HandleDeadOnRigidBody()
    {
        // Rigidbody의 모든 Constraints 해제
        rb.constraints = RigidbodyConstraints.None;

        // Collider 비활성화
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
        Spin();
    }
}