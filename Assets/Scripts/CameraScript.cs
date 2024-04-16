using UnityEngine;

public class CameraScript : MonoBehaviour
{

    // Fields

    public float zoomSpeed = 0.1f;
    public float minZoomSize = 1.5f;
    public float maxZoomSize = 3.5f;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    // Class Methods
    
    void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > minZoomSize) {
            Camera.main.orthographicSize -= zoomSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < maxZoomSize) {
            Camera.main.orthographicSize += zoomSpeed;
        }

        if (Input.GetMouseButtonDown(1)) {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(1)) {
            isDragging = false;
        }

        if (isDragging) {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            transform.Translate(-deltaMousePosition * Time.deltaTime);
            lastMousePosition = Input.mousePosition;
        }
    }
}

