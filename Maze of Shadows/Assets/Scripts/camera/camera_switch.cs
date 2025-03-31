using UnityEngine;

public class CameraTriggerZone : MonoBehaviour
{
    public Camera targetCamera;
    public Camera[] allCameras;

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

        Debug.Log(">>> Triggered by PLAYER. Switching camera to: " + targetCamera.name);

        foreach (Camera cam in allCameras)
        {
            if (cam != null)
            {
                cam.enabled = false; // Turn off the Camera component
                cam.clearFlags = CameraClearFlags.SolidColor; // Prevent ghost frames
                Debug.Log("Disabled camera: " + cam.name);
            }
        }

        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.enabled = true; // Turn on only the target camera
            Debug.Log("Activated camera: " + targetCamera.name);
        }
    }
}
