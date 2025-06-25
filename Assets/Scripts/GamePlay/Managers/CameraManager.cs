using UnityEngine;
using System.Collections.Generic;
using System.Security;

public enum CameraType
{
    MainCamera,
    GameEndCamera,
}

[System.Serializable]
public class TypedCamera
{
    public CameraType type;
    public Camera camera;
}

public class CameraManager : SingletonBehaviour<CameraManager>
{
    [SerializeField] private Camera currentMainCamera;
    [SerializeField] private List<TypedCamera> cameraList;
    
    protected override void Init()
    {
        IsDestroyOnLoad = true;
        base.Init();
        SwitchToCamera(CameraType.MainCamera);
    }

    public void SwitchToCamera(CameraType type)
    {
        foreach(var camera in cameraList)
        {
            camera.camera.gameObject.SetActive(camera.type == type);
            if (camera.type == type)
            {
                currentMainCamera = camera.camera;
            }
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator<WaitForSeconds> ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = currentMainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            currentMainCamera.transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        currentMainCamera.transform.localPosition = originalPos;
    }
}
