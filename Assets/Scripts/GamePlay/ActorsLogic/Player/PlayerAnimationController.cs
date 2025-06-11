using UnityEngine;

public class PlayerAnimationController : AnimalAnimationController
{
    private Player owner;
    [SerializeField] private Transform playerModelHolder;

    public void Init(Player owner)
    {
        this.owner = owner;
        LoadPlayerModel();
        base.Init();

    }

    private void LoadPlayerModel()
    {
        ClearCharacterModel();
        InstantiatePlayerCharacterModel();
    }

    private void ClearCharacterModel()
    {
        for(int i = 0; i < playerModelHolder.childCount; i++)
        {
            Destroy(playerModelHolder.GetChild(0).gameObject);
        }
    }

    private void InstantiatePlayerCharacterModel()
    {
        GameObject model = Instantiate(UnlockSaveManager.Instance.GetSelectedUnlockData().UnlockPrefab, playerModelHolder);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;
        animator = model.GetComponent<Animator>();
    }
    public void ChangeAnimationDependOnSpeed()
    {
        float speed = owner.Stat.RigidbodySpeed;
        if (speed > float.Epsilon)
        {
            float ratio = owner.Stat.RigidbodySpeed / owner.Stat.StandardAnimationSpeed;
            ChangeAnimation(AnimalAnimation.Run);
        }
        else
        {
            ChangeAnimation(AnimalAnimation.Idle_A);
        }
    }
}
