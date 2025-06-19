using UnityEngine;

public class PlayerFeverHandler : MonoBehaviour
{
    public float feverCount = 0;
    private float feverMax = 15f;
    public float currentFeverMax = 0;
    
    private float currentCooltime = 1f;
    private float fevercoolTime = 2f;
    private float fevercoolRatioPerSec = 0.05f;

    public float feverTime = 0f;
    private float originalFeverTime = 4f;
    public float currentFeverTimeMax = 3f;
    
    public bool isFever = false;

    public void AddFeverGauge(float expAmount)
    {
        feverCount += expAmount;
        currentCooltime = 0;

        if(feverCount >= GetFeverLimit())
        {
            isFever = true;
            feverTime = currentFeverTimeMax;
            feverCount = 0;
        }
    }

    private void Update()
    {
        if(GameManager.Instance.IsGameStarted || !GameManager.Instance.IsGamePaused)
        {
            currentFeverMax = GetFeverLimit();
            if (isFever)
            {
                feverTime -= Time.deltaTime;
                if (feverTime < 0)
                {
                    isFever = false;
                }
            }
            else
            {
                currentCooltime += Time.deltaTime;
                if (currentCooltime >= fevercoolTime)
                {
                    CooldownFever();
                }
            }
        }
    }

    private void CooldownFever()
    {
        float FeverCoolPerSec = GetFeverLimit() * fevercoolRatioPerSec;
        feverCount -= FeverCoolPerSec * Time.deltaTime;
        if(feverCount < 0 )
        {
            feverCount = 0;
        }
    }

    

    public float GetFeverLimit()
    {
        return feverMax * GameManager.Instance.EnemyHealthRatio.Ratio;
    }

    // fever 중일때에는 fever time을 
    // fever이 아닐 때에는 fever gauge를...
    public float GetFeverRatio()
    {
        if(isFever)
        {
            return feverTime / currentFeverTimeMax;
        }
        else
        {
            if(feverCount == 0)
            {
                return 0f;
            }
            return feverCount / GetFeverLimit();
        }
    }
}
