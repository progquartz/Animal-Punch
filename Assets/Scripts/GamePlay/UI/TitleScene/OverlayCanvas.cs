using UnityEngine;

public class OverlayCanvas : MonoBehaviour
{
    public static OverlayCanvas instance;

    [SerializeField]
    private UnderDevelopmentUI underDevelopmentUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void CallUnderDevelopmentUI()
    {
        underDevelopmentUI.StartReveal();
    }

    public void TestButton()
    {
        
     //   GameManager.Instance.GetPlayerInfoData().ClearExp();
    }
    
}
