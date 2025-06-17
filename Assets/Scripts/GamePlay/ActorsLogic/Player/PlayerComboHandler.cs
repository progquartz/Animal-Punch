using UnityEngine;

public class PlayerComboHandler : MonoBehaviour
{
    public int comboCount = 0;
    public float comboTimer = 0f;
    public float comboResetTime = 3.0f;

    public bool IsComboActive = false;

    public ComboUI comboUI;


    void Update()
    {
        if (IsComboActive)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= comboResetTime)
            {
                ResetCombo();
            }
        }
    }

    public void UpdateCombo()
    {
        if (!IsComboActive || comboTimer >= comboResetTime)
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