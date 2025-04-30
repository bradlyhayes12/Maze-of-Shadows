using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CameraTriggerZoneTest
{
    [UnityTest]
    public IEnumerator CameraSwitchesToTargetCorrectly()
    {
        // Create player
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<BoxCollider2D>(); // ✅ Add Collider2D for trigger interaction

        // Create and set up camera
        GameObject camObj = new GameObject("TestCamera");
        Camera targetCam = camObj.AddComponent<Camera>();
        targetCam.enabled = false;

        // Create trigger zone
        GameObject triggerZone = new GameObject("TriggerZone");
        BoxCollider2D collider = triggerZone.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        CameraTriggerZone script = triggerZone.AddComponent<CameraTriggerZone>();
        script.targetCamera = targetCam;

        // Simulate trigger
        triggerZone.transform.position = Vector3.zero;
        player.transform.position = Vector3.zero;

        // Manually invoke the trigger event
        script.SendMessage("OnTriggerEnter2D", player.GetComponent<Collider2D>(), SendMessageOptions.DontRequireReceiver);

        yield return null;

        Assert.IsTrue(targetCam.enabled, "Camera should have been enabled after trigger.");
    }
}
