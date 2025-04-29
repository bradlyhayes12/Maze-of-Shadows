using UnityEngine;

public class Arrow_stick : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Stick the arrow into what it hits (optional)
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
