using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerExpGauge : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private Vector3 targetOffset = new Vector3(0f, -1f, 0f);

    private float MinArcRotation = 345f;
    private float MaxArcRotation = 15f;
    private float TotalArcRange = 330f;
    private float prevRatio = 0f;
    private float followUpLerpTime = 0.3f;

    private void Update()
    {
        UpdatePosition();
        UpdateExpGauge();
    }

    private void UpdatePosition()
    {
        transform.position = Target.position + targetOffset;
    }

    private void UpdateExpGauge()
    {
        float ratio = (float)Player.Instance.Stat.CurrentExp / Player.Instance.Stat.LevelUpExpNeed;
        if (ratio < prevRatio) // 레벨업 한 경우
        {
            prevRatio = ratio;
            gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Arc2", CalculateArcRotation(ratio));
        }
        else // 레벨업하지 않는 경우, followuplerptime에 따라서 부드럽게 exp가 증가.
        {
            prevRatio = Mathf.Lerp(prevRatio, ratio, Time.deltaTime / followUpLerpTime);
            gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Arc2", CalculateArcRotation(prevRatio));
        }
        
    }

    private float CalculateArcRotation(float ratio)
    {
        return MinArcRotation - (TotalArcRange * ratio);
    }
}
