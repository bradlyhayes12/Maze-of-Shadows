using UnityEngine;

public class CameraTriggerZone : MonoBehaviour
{
    public Camera targetCamera;
    public bool ignoreFirstTrigger = false;

    private bool hasTriggeredOnce = false;

    void Start()
    {
        Debug.Log("CameraTriggerZone script is active on: " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D hit by: " + other.name + ", tag: " + other.tag);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Ignored because tag was not Player");
            return;
        }

        if (ignoreFirstTrigger && !hasTriggeredOnce)
        {
            hasTriggeredOnce = true;
            Debug.Log("🟡 Ignored first trigger (spawn room)");
            return;
        }

        Debug.Log(">>> Triggered by PLAYER. Switching camera to: " + targetCamera?.name);

        //Find all active cameras at runtime
        Camera[] allCameras = Camera.allCameras;

        Debug.Log("Found " + allCameras.Length + " active cameras at runtime:");
        foreach (Camera cam in allCameras)
        {
            Debug.Log($" - Camera: {cam.name}, Enabled: {cam.enabled}, Position: {cam.transform.position}");
        }

        // Disable all cameras
        foreach (Camera cam in allCameras)
        {
            if (cam != null)
            {
                cam.enabled = false;
                cam.clearFlags = CameraClearFlags.SolidColor;
                Debug.Log("Disabled camera: " + cam.name);
            }
        }

        //Enable the target camera
        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.enabled = true;
            Debug.Log("✅ Activated camera: " + targetCamera.name);
        }

        hasTriggeredOnce = true;
    }
}
