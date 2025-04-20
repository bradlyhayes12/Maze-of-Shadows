using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    public Vector3 targetPosition;
    public float targetSize = 5f;
    public float moveSpeed = 3f;
    public float zoomSpeed = 5f;

    private Camera cam;
    private bool zooming = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void StartZoom(Vector3 roomCenter)
    {
        targetPosition = new Vector3(roomCenter.x, roomCenter.y, transform.position.z);
        zooming = true;
    }

    void Update()
    {
        if (!zooming) return;

        // Smooth move
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Smooth zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        // Optional: stop zooming when close enough
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f &&
            Mathf.Abs(cam.orthographicSize - targetSize) < 0.1f)
        {
            zooming = false;
            Debug.Log("✅ Camera zoom complete");
        }
    }
}
