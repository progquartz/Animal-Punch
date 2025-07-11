using UnityEngine;

public class PlayerComboHandler : MonoBehaviour
{
    public int comboCount = 0;
    public float comboDamageRatio = 0f;
    public float comboTimer = 0f;
    public float comboResetTime = 3.0f;


    public bool IsComboActive = false;

    public ComboUI comboUI;


    void Update()
    {
        if(GameManager.Instance.IsGameStarted || !GameManager.Instance.IsGamePaused)
        {
            // ratio 계산은 데미지 비율 증가가 언제든 생길 수 있어서 update에 둠.
            comboDamageRatio = 1.0f + (comboCount * Player.Instance.Stat.GetComboCountRatio());
            if (IsComboActive)
            {
                comboTimer += Time.deltaTime;

                if (comboTimer >= comboResetTime + comboBonusTime)
                {
                    ResetCombo();
                }
            }
        }
    }

    public void UpdateCombo()
    {
        if (!IsComboActive || comboTimer >= comboResetTime + comboBonusTime)
        {
            comboCount = 1; 
        }
        else
        {
            comboCount++;
        }

        comboTimer = 0f; 
        IsComboActive = true;
        if(comboUI != null)
        {
            comboUI.OnComboUpdate();
        }
    }


    private void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 0f;
        IsComboActive = false;
    }
}