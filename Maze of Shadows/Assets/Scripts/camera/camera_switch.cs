using UnityEngine;
using System.Collections.Generic;

public class CameraTriggerZone : MonoBehaviour
{
    public Camera targetCamera;
    public bool ignoreFirstTrigger = false;

    [Header("Enemy Spawning")]
    public GameObject samuraiPrefab;
    public Transform[] spawnPoints;

    private bool hasTriggeredOnce = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

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

        // Find and disable all active cameras
        Camera[] allCameras = Camera.allCameras;
        Debug.Log("Found " + allCameras.Length + " active cameras at runtime:");
        foreach (Camera cam in allCameras)
        {
            Debug.Log($" - Camera: {cam.name}, Enabled: {cam.enabled}, Position: {cam.transform.position}");
            if (cam != null)
            {
                cam.enabled = false;
                cam.clearFlags = CameraClearFlags.SolidColor;
                Debug.Log("Disabled camera: " + cam.name);
            }
        }

        // Enable the target camera
        if (targetCamera != null)
        {
            targetCamera.clearFlags = CameraClearFlags.SolidColor;
            targetCamera.enabled = true;
            Debug.Log("✅ Activated camera: " + targetCamera.name);
        }

        // Spawn samurai enemies
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (samuraiPrefab != null)
            {
                GameObject enemy = Instantiate(samuraiPrefab, spawnPoint.position, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                Debug.Log("Spawned enemy at: " + spawnPoint.position);
            }
        }

        hasTriggeredOnce = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Destroy all spawned enemies when the player exits the room
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();

        Debug.Log("Destroyed all enemies because player exited the room.");
    }
}
