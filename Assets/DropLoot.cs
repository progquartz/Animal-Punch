using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public Transform Target;
    public Transform Loot;
    public float followingDistance = 10f;
    public float MinModifier = 7f;
    public float MaxModifier = 11;
    private string animatorTriggerString = "IsTriggered";

    public float DisappearingTime = 5.0f;

    Vector3 _velocity = Vector3.zero;
    bool isFollowing = false;

    
    void Start()
    {
        Target = Player.Instance.PlayerTransform;
    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

    void Update()
    {
        if(isFollowing)
        {
            Loot.transform.position = Vector3.SmoothDamp(Loot.transform.position, Target.position, ref _velocity, Time.deltaTime * Random.Range(MinModifier, MaxModifier));
        }
    }
}
