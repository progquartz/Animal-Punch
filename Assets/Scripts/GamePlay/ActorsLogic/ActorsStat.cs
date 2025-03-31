using UnityEngine;

[System.Serializable]
public class ActorsStat : MonoBehaviour
{
    [Header("�̸�")]
    public string ActorName;
    public GameObject ModelData;

    [Header("ü��")]
    public bool IsDead = false;
    public float HP;
    public float MaxHP;

    [Header("���� ����")]
    // ���� Ŭ���� ����.

    [Header("������ �Ӽ�")]
    public float Speed;
    public float Mass;
    public float MassOnDead;
    public float Drag;
    public float DragOnDead;
    public float AngularDrag;
    public float AngularDragOnDead;


    /// <summary>
    /// �������� ���. ���� ���, true�� ����.
    /// </summary>
    public bool HandleDamage(float damage)
    {
        Debug.Log($"{damage}��ŭ�� �������� {HP}�� ���մϴ�.");
        if(IsDead) return false;
        // �� �׾��� ���
        if(HP - damage > 0)
        {
            HP -= damage;
            return false;
        }
        else
        {
            HP = 0;
            IsDead = true;
            return true;
        }
    }

}
