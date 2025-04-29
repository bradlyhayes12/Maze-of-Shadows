using UnityEngine;

public class ArrowLifetime : MonoBehaviour
{
    public float lifetime = 2.5f;  // How long before the arrow disappears

    void Start()
    {
        Destroy(gameObject, lifetime);  // Destroy this arrow after "lifetime" seconds
    }
}
