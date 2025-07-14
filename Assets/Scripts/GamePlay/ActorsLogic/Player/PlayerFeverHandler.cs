using UnityEngine;

public class PlayerFeverHandler : MonoBehaviour
{
    public float feverGauge = 0;
    private float feverMax = 15f;
    public float currentFeverMax = 0;
    
    private float currentCooltime = 1f;
    private float fevercoolTime = 2f;
    private float fevercoolRatioPerSec = 0.05f;

    public float feverTime = 0f;
    public float originalFeverTime = 3f; // looting에 추가할 때에 사용해야 함.
    public float feverTimeDuration = 3f;
    
    
    public bool isFever = false;

    public void AddFeverGauge(float expAmount)
    {
        expAmount = expAmount * (1 + (0.01f * Player.Instance.Stat.FeverGaugeBonus));

        feverGauge += expAmount;
        currentCooltime = 0;

        if(feverGauge >= GetFeverLimit())
        {
            isFever = true;
            feverTime = feverTimeDuration + Player.Instance.Stat.FeverBonusTime;
            feverGauge = 0;
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
        feverGauge -= FeverCoolPerSec * Time.deltaTime;
        if(feverGauge < 0 )
        {
            feverGauge = 0;
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
            return feverTime / (feverTimeDuration + Player.Instance.Stat.FeverBonusTime);
        }
        else
        {
            if(feverGauge == 0)
            {
                return 0f;
            }
            return feverGauge / GetFeverLimit();
        }
    }
}
