using UnityEditorInternal;
using UnityEngine;

public class InteractableActor : MonoBehaviour
{
    private Enemy owner;
    private bool isActorAbleToHit = true;

    public float DamageMinimalCooldown = 0.1f;
    private float damageCooldown;

    [SerializeField] private Transform ActorTransform;
    private Rigidbody rb;

    private Collider objectCollider;
    

    public void Init(Enemy enemy)
    {
        owner = enemy;
        rb = ActorTransform.GetComponent<Rigidbody>();
        objectCollider = ActorTransform.transform.GetComponent<Collider>();
        damageCooldown = DamageMinimalCooldown;
    }

    private void Update()
    {
        UpdateCooldown();
    }

    public void HandleCollision(Collision collision, GameObject collisionObject, float impulseDamage)
    {
        if (collisionObject.CompareTag("Player"))
        {
            if (owner.IsEnemyHasHealth)
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
        if(isActorAbleToHit)
        {
            bool isDead = owner.stat.HandleDamage(impulseDamage);
            DamageTextPooler.Instance.SpawnDamageText(impulseDamage, ActorTransform.position);
            if (isDead)
            {
                HandleMassForceChange(collision);
                HandleShootingUp();
                HandleDeadOnRigidBody();
                Spin();
            }
            else
            {

            }
        }
    }

    private void HandleMassForceChange(Collision collision)
    {
        // 충돌 시 발생한 impulse. 충돌한 객체로부터 받아낸 impulse이기에 -1을 곱해야 함.
        Vector3 impulse = -collision.impulse / 30;

        // 현재 운동 상태 초기화
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 질량 변경
        rb.mass = owner.stat.MassOnDead;
        rb.linearDamping = owner.stat.DragOnDead;
        rb.angularDamping = owner.stat.AngularDragOnDead;

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

    private void UpdateCooldown()
    {
        if(!isActorAbleToHit)
        {
            if(damageCooldown < 0)
            {
                damageCooldown = DamageMinimalCooldown;
                isActorAbleToHit = true;
                return;
            }
            damageCooldown -= Time.deltaTime;
        }
    }
    private void HandleDeath()
    {
        // 만약 연결해야 할 부분이 있거나 추가로 처리해야 하는 부분이 있다면 추후 처리.
        Destroy(this.gameObject, 1f);
    }
}