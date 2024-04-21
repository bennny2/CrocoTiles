using UnityEngine;

public class CameraScript : MonoBehaviour
{

    // Fields

    private float _zoomSpeed = 0.1f;
    private float _minZoomSize = 1.5f;
    private float _maxZoomSize = 3.5f;
    private Vector3 _lastMousePosition;
    private bool _isDragging = false;

    public float ZoomSpeed { get => _zoomSpeed; set => _zoomSpeed = value; }
    public float MinZoomSize { get => _minZoomSize; set => _minZoomSize = value; }
    public float MaxZoomSize { get => _maxZoomSize; set => _maxZoomSize = value; }
    public Vector3 LastMousePosition { get => _lastMousePosition; set => _lastMousePosition = value; }
    public bool IsDragging { get => _isDragging; set => _isDragging = value; }

    // Class Methods

    void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > MinZoomSize) {
            Camera.main.orthographicSize -= ZoomSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < MaxZoomSize) {
            Camera.main.orthographicSize += ZoomSpeed;
        }

        if (Input.GetMouseButtonDown(1)) {
            LastMousePosition = Input.mousePosition;
            IsDragging = true;
        }
        else if (Input.GetMouseButtonUp(1)) {
            IsDragging = false;
        }

        if (IsDragging) {
            Vector3 deltaMousePosition = Input.mousePosition - LastMousePosition;
            transform.Translate(-deltaMousePosition * Time.deltaTime);
            LastMousePosition = Input.mousePosition;
        }
    }
}

