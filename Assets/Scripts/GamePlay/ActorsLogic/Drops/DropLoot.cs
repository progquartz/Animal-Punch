using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [SerializeField] private DropItemData dropItemData;
    public Transform Target;
    public Transform Loot;
    [SerializeField] private Animator animator;

    public float waitingTimeAfterDrop = 0.8f;
    public float DisappearingTime = 5.0f;

    private float currentDisappearingTime;
    private float currentWaitingTime;

    
    private bool currentWaiting = true;
    public float followingDistance = 5f;
    public float MinModifier = 3f;
    public float MaxModifier = 4f;
    private string animatorTriggerString = "IsFollowing";
    private string animatorResetString = "IsReset";


    Vector3 _velocity = Vector3.zero;
    bool isFollowing = false;

    public void Init(DropItemData dropData)
    {
        this.dropItemData = dropData;
        if (Target == null)
        {
            Target = Player.Instance.PlayerTransform;
        }
        animator.SetTrigger(animatorResetString);
        currentWaitingTime = waitingTimeAfterDrop;
        currentWaiting = true;
        isFollowing = false;
    }
    void Start()
    {
        // 추후에 초기화 필요.
        Init(dropItemData);
    }


    public void StartFollowing()
    {
        isFollowing = true;
        animator.SetBool(animatorTriggerString, true);
    }

    private void CalculateWaitingTime()
    {
        currentWaitingTime -= Time.deltaTime;
        if (currentWaitingTime < 0f)
        {
            currentWaiting = false;
        }
    }

    private void CalculateDestroyingTime()
    {
        // 따라가는 순간부터는 삭제 안시킴.
        if(!isFollowing)
        {
            currentDisappearingTime -= Time.deltaTime;
            if (currentDisappearingTime < 0f)
            {
                // 스스로 삭제 (또는 풀로 돌아가게 만들기)
            }

        }
    }

    private void CalculateFollowing()
    {
        if (!isFollowing)
        {
            float distance = Vector3.Distance(Target.position, Loot.position);
            if (distance < followingDistance)
            {
                Debug.Log("따라갑니다!");
                StartFollowing();
            }
        }
        else
        {
            Loot.position = Vector3.SmoothDamp(Loot.position, Target.position, ref _velocity, Time.deltaTime * Random.Range(MinModifier, MaxModifier));
        }
    }

    private void LootItem()
    {
        Player.Instance.Stat.GainExp(dropItemData.ExpAmount);
        Player.Instance.Inventory.GainGold(dropItemData.GoldAmount);
        if(dropItemData.IsGainingItem)
        {
            // 보물상자 열리는 루팅 열기.
        }
    }

    void Update()
    {
        if(currentWaiting)
        {
            CalculateWaitingTime();
            CalculateDestroyingTime();
        }
        else
        {
            CalculateFollowing();
            CalculateDestroyingTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            LootItem();
            Destroy(this.gameObject);
        }
    }
}
