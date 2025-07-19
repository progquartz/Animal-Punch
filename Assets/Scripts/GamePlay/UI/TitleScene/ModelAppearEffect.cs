using System.Collections;
using UnityEngine;

public class ModelAppearEffect : MonoBehaviour
{    
    public float duration = 0.5f;            // 전체 등장 시간
    public int yRotationRounds = 5;          // y축 회전 횟수 (3바퀴 = 1080도)

    private Vector3 initialScale = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 targetScale = Vector3.one;

    private void OnEnable()
    {
        // 초기 상태 설정
        transform.localScale = initialScale;
        StartCoroutine(PlayAppearAnimation());
    }

    private IEnumerator PlayAppearAnimation()
    {
        float elapsed = 0f;
        float totalRotation = 360f * yRotationRounds;
        float startYRotation = transform.eulerAngles.y;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 회전: y축 기준으로 빠르게 회전
            float yRot = Mathf.Lerp(0, totalRotation, t);
            transform.rotation = Quaternion.Euler(0f, startYRotation + yRot, 0f);

            // 스케일 점점 키우기
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        // 보정 (정확하게 마지막 상태로 설정)
        transform.rotation = Quaternion.Euler(0f, startYRotation + totalRotation, 0f);
        transform.localScale = targetScale;
    }
}
