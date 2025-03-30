using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    [Header("���� ����")]
    public float RigidbodySpeed = 0f;               // �ӵ�
    public float CurrentAdditionForce = 0f;         // ���� ���ӵ�
    public float LastDashTime;                      // ���� �뽬 ��Ÿ��

    [Header("�̵� �⺻ �߷�")]
    public float MoveForce;                         // �̵��� �� �������� ��

    [Header("�ν���")]
    public float BoostForce;                        // ������ Ƣ��� ��
    public float BoostCooltime;                     // Space Ű ��Ÿ��

    [Header("���ӵ�")]
    public float AdditionForceRatio;                // �߰� ���ӵ� ����
    public float AdditionForceMax;                  // �ִ� �߰� ���ӵ�

    [Header("ȸ��")]
    public float RotationSpeed = 1000f;             // �ִ� ȸ�� �ӵ�.

    [Header("��� ������")]
    public float CollisionDamageBase;               // �⺻ ������
    public float CollisionDamageAdditional;         // �߰� ������
    public float CollisionImpulseDamageBase;
    public float CollisionImpulseStandard;          // ���� ��ݷ� (1��)
    public float CollisionImpulseDamageRatio;       // ��ݷ� ��� �߰� ������
}
