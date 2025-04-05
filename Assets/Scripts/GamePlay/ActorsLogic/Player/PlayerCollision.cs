using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerPhysics owner;

    private Transform playerTransform;
    private Rigidbody playerRB;
    

    // 충돌 전 속도를 저장할 변수
    private Vector3 previousVelocity;

    public void Init(PlayerPhysics owner)
    {
        this.owner = owner;
        playerRB = owner.playerRB;
        playerTransform = owner.playerTransform;
    }

    private void FixedUpdate()
    {
        previousVelocity = playerRB.linearVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Interactable"))
        {
            ActorCollision interactable = collision.gameObject.GetComponent<ActorCollision>();
            if(interactable != null)
            {
                // 충돌할 경우 속도가 줄어들기 때문에, 이를 기반으로 줄어든 속도 = 충격량으로 데미지 계산을 측정.
                // 단순 충격량을 하지 않는 이유는, 이를 받아내는 객체의 무게 계산까지 해야 하기 때문.
                Vector3 velocityChanged = playerRB.linearVelocity - previousVelocity;
                float impulseMagnitude = velocityChanged.magnitude;

                //Debug.Log($"충격이 일어났으며, 그 충격량은 {impulseMagnitude}입니다.");
                float impulseDamage = owner.CalculateImpulseDamage(impulseMagnitude);
                interactable.HandleCollision(collision, playerTransform.gameObject, impulseDamage);
            }
        }
    }
}
