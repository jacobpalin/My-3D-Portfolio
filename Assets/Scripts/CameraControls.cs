using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform target; // The object to orbit around
    public float rotationSpeed = 10f; // Rotation speed for mouse
    public float touchRotationSpeed = 0.2f; // Rotation speed for touch
    public float distance = 5f; // Initial distance
    public float minDistance = 2f; // Minimum zoom distance
    public float maxDistance = 15f; // Maximum zoom distance
    public float zoomSpeed = 2f; // Zoom speed for mouse
    public float pinchZoomSpeed = 0.1f; // Zoom speed for pinch

    private Vector3 currentRotation; // Current rotation angles
    private Vector2 lastTouchPosition; // Last touch position for touch input
    private bool isRotating = false; // Whether the camera is currently being rotated
    private float previousPinchDistance; // Last pinch distance for zooming

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned.");
            return;
        }

        currentRotation = transform.eulerAngles;
        UpdateCameraPosition();
    }

    void Update()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            HandleMouseInput(); // Desktop and WebGL
        }
        else
        {
            HandleTouchInput(); // Mobile
        }
    }

    void HandleMouseInput()
    {
        // Rotation
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(0)) // Release left click
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            currentRotation.y += mouseX * rotationSpeed;
            currentRotation.x -= mouseY * rotationSpeed;

            // Clamp vertical rotation
            currentRotation.x = Mathf.Clamp(currentRotation.x, -85f, 85f);

            UpdateCameraPosition();
        }

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            UpdateCameraPosition();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1) // Single touch for rotation
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                lastTouchPosition = touch.position;

                currentRotation.y += delta.x * touchRotationSpeed;
                currentRotation.x -= delta.y * touchRotationSpeed;

                // Clamp vertical rotation
                currentRotation.x = Mathf.Clamp(currentRotation.x, -85f, 85f);

                UpdateCameraPosition();
            }
        }
        else if (Input.touchCount == 2) // Pinch for zoom
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousPinchDistance = currentPinchDistance;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float pinchDelta = currentPinchDistance - previousPinchDistance;
                previousPinchDistance = currentPinchDistance;

                distance -= pinchDelta * pinchZoomSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);

                UpdateCameraPosition();
            }
        }
    }

    void UpdateCameraPosition()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(currentRotation);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}