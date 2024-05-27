using UnityEngine;

public class CameraScript : MonoBehaviour
{

    // Fields

    private float _zoomSpeed = 0.1f;
    private float _minZoomSize = 1.5f;
    private float _maxZoomSize = 3.5f;

    // Properties

    public float ZoomSpeed { get => _zoomSpeed; set => _zoomSpeed = value; }
    public float MinZoomSize { get => _minZoomSize; set => _minZoomSize = value; }
    public float MaxZoomSize { get => _maxZoomSize; set => _maxZoomSize = value; }

    // Class Methods

    void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > MinZoomSize) {
            Camera.main.orthographicSize -= ZoomSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < MaxZoomSize) {
            Camera.main.orthographicSize += ZoomSpeed;
        }
    }
}

