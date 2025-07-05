using UnityEngine;

public class ActorCollision : MonoBehaviour
{
    private Enemy owner;
    private bool isActorAbleToHit = true;
    private bool isFirstTimeSpawn = false;

    public float DamageMinimalCooldown = 0.1f;
    private float damageCooldown;

    private Transform ActorTransform;
    private Rigidbody rb;

    private BoxCollider objectCollider;
    

    public void Init(Enemy enemy, Transform actorTransform)
    {
        isFirstTimeSpawn = true;
        owner = enemy;


        ActorTransform = actorTransform;

        InitializeRigidBody();
        InitializeCollider();
        damageCooldown = DamageMinimalCooldown;
    }

    private void InitializeRigidBody()
    {
        rb = ActorTransform.GetComponent<Rigidbody>();

        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        rb.mass = owner.stat.Mass;
        rb.angularDamping = owner.stat.AngularDrag;
        rb.linearDamping = owner.stat.Drag;
    }

    private void InitializeCollider()
    {
        objectCollider = ActorTransform.transform.GetComponent<BoxCollider>();
        objectCollider.enabled = true;
        objectCollider.size = new Vector3(owner.targetEnemyDataSO.ModelColliderScale.x, owner.targetEnemyDataSO.ModelColliderScale.y ,owner.targetEnemyDataSO.ModelColliderScale.z);
    }


    private void Update()
    {
        if(isFirstTimeSpawn)
        {
            CheckCollisionOnSpawn();
        }
        UpdateCooldown();
    }

    public bool HandleCollision(Collision collision, float impulseDamage, bool isCritical)
    {
        bool isDead = false;   
        if (owner.targetEnemyDataSO.IsEnemyHasHealth)
        {
            isDead = HandleCollisionOnHealthCondition(collision, impulseDamage, isCritical);
        }
        else
        {
            isDead = HandleCollisionOnNoneHealthCondition(collision);
        }
        return isDead;
    }


    private bool HandleCollisionOnHealthCondition(Collision collision,  float impulseDamage, bool isCritical)
    {
        if(isActorAbleToHit)
        {
            Debug.Log($"플레이어와 {owner.targetEnemyDataSO.ActorName}간의 충격량이 {impulseDamage} 입니다?");
            bool isDead = owner.HandleDamage(collision, impulseDamage, isCritical);
            if (isDead)
            {
                HandleDeadOnRB(collision);
            }
            return isDead;
        }
        return false;
    }

    private bool HandleCollisionOnNoneHealthCondition(Collision collision)
    {
        HandleDeadOnRB(collision);
        return true;
    }


    private void HandleDeadOnRB(Collision collision)
    {
        HandleDeathForce(collision);
        HandleShootingUp();
        HandleDeadOnRigidBody();
    }

    private void HandleDeathForce(Collision collision)
    {
        // 충돌 시 발생한 impulse. 충돌한 객체로부터 받아낸 impulse이기에 -1을 곱해야 함.
        Vector3 impulse = -collision.impulse / 10;

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

    private void CheckCollisionOnSpawn()
    {
        isFirstTimeSpawn = false;
    }
}