using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public float minZoomSize = 1.5f;
    public float maxZoomSize = 3.5f;

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > minZoomSize)
        {
            Camera.main.orthographicSize -= zoomSpeed;
        }

        // Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Camera.main.orthographicSize < maxZoomSize)
        {
            Camera.main.orthographicSize += zoomSpeed;
        }

        // Dragging
        if (Input.GetMouseButtonDown(1))
        {
            // Record the position of the mouse when the left mouse button is clicked
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            // Stop dragging when the left mouse button is released
            isDragging = false;
        }

        if (isDragging)
        {
            // Calculate the difference in mouse position since the last frame
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;

            // Adjust the camera's position based on the mouse movement
            transform.Translate(-deltaMousePosition * Time.deltaTime);

            // Update the last mouse position to the current mouse position
            lastMousePosition = Input.mousePosition;
        }
    }
}

