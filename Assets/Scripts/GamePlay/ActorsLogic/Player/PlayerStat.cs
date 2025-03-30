using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    [Header("현재 상태")]
    public float RigidbodySpeed = 0f;               // 속도
    public float CurrentAdditionForce = 0f;         // 현재 가속도
    public float LastDashTime;                      // 남은 대쉬 쿨타임

    [Header("이동 기본 추력")]
    public float MoveForce;                         // 이동할 때 가해지는 힘

    [Header("부스터")]
    public float BoostForce;                        // 앞으로 튀어나갈 힘
    public float BoostCooltime;                     // Space 키 쿨타임

    [Header("가속도")]
    public float AdditionForceRatio;                // 추가 가속도 비중
    public float AdditionForceMax;                  // 최대 추가 가속도

    [Header("회전")]
    public float RotationSpeed = 1000f;             // 최대 회전 속도.
}
