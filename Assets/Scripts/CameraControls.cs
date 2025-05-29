using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    public Transform target;             // The object to orbit around
    public GameObject canvasObj;
    public DeviceDetector detector;
    public float defaultRotationSpeed; // Default rotation speed
    public float zoomSpeed;         // Speed of zooming
    public float minZoom;            // Minimum zoom distance
    public float maxZoom;           // Maximum zoom distance
    public float transitionDuration; // Duration of the smooth transition
    public List<Transform> islands;      // List of island transforms

    private float rotationSpeed;          // Current rotation speed
    private Vector3 offset;               // Offset from the target to the camera
    private float currentZoom;            // Current zoom distance
    private float currentRotationY;       // Current Y-axis rotation
    private float currentRotationX;       // Current X-axis rotation
    private bool isCycling;               // Tracks if the camera is in transition
    public int currentIslandIndex;       // Index of the currently focused island

    private bool isPointerOverUI;         // Tracks if the pointer is over any slider
    public Transform startPosition;


    void Start()
    {
        if (detector.CheckIfMobile() == false)
        {
            defaultRotationSpeed = 700;
        }

        transform.position = startPosition.position;
        transform.rotation = Quaternion.Euler(startPosition.eulerAngles.x, startPosition.eulerAngles.y, startPosition.eulerAngles.z);

        offset = transform.position - target.position;
        currentZoom = offset.magnitude;

        rotationSpeed = defaultRotationSpeed;
        currentRotationX = transform.eulerAngles.x;
        currentRotationY = transform.eulerAngles.y;

        currentIslandIndex = 0; // Start at the first island
        StartCoroutine(CycleTransition(currentIslandIndex));
    }

    void Update()
    {
        if (isCycling) return;

        // Handle rotation with mouse input
        if (!isPointerOverUI && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            currentRotationY += mouseX;
            currentRotationX -= mouseY;
            currentRotationX = Mathf.Clamp(currentRotationX, -85f, 85f);
        }

        // Handle zoom with scroll wheel (desktop)
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (scroll != 0 && !isPointerOverUI)
        {
            currentZoom = Mathf.Clamp(currentZoom - scroll, minZoom, maxZoom);
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            currentZoom = Mathf.Clamp(currentZoom - deltaMagnitudeDiff * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        }

        // Handle island cycling via keyboard input
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            CycleToIsland(currentIslandIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            CycleToIsland(currentIslandIndex - 1);
        }
    }

    void LateUpdate()
    {
        if (isCycling) return;

        // Calculate new position based on current rotation and zoom
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 newOffset = rotation * Vector3.back * currentZoom;

        // Set camera position and rotation
        transform.position = target.position + newOffset;
        transform.LookAt(target);
    }

    private void OnZoomSliderChanged(float value)
    {
        currentZoom = value;
    }

    private void OnRotationSpeedSliderChanged(float value)
    {
        rotationSpeed = value;
    }

    public void CycleToIsland(int newIndex)
    {
        if (isCycling || islands.Count == 0) return;

        // Wrap around the index
        newIndex = (newIndex + islands.Count) % islands.Count;

        StartCoroutine(CycleTransition(newIndex));
    }

    private IEnumerator CycleTransition(int newIndex)
    {
        isCycling = true;
        canvasObj.SetActive(false);

        Transform nextIsland = islands[newIndex];
        Vector3 directionToIsland = -(nextIsland.position - target.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToIsland, Vector3.up);
        Quaternion initialRotation = transform.rotation;

        // Define how long to spend on each phase
        float zoomOutTime = .6f;
        float rotateTime = .8f;
        float zoomInTime = .6f;
        float elapsedTime = 0f;
        float zoomInTarget = minZoom;
        float zoomOutTarget = 20f;
        float lastZoom = currentZoom;

        // Zoom out smoothly before rotating
        while (elapsedTime < zoomOutTime)
        {
            elapsedTime += Time.deltaTime;
            currentZoom = Mathf.Lerp(lastZoom, zoomOutTarget, elapsedTime / zoomOutTime);
            Vector3 smoothedOffset = transform.forward * -currentZoom;
            transform.position = target.position + smoothedOffset;

            yield return null;
        }

        // Smooth rotation and movement to the next island
        elapsedTime = 0f;
        while (elapsedTime < rotateTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotateTime;
            Quaternion smoothRotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            Vector3 smoothedOffset = smoothRotation * Vector3.forward * -currentZoom;
            transform.position = target.position + smoothedOffset;
            transform.LookAt(target);

            yield return null;
        }

        // Zoom back in after rotation is done
        elapsedTime = 0f;
        while (elapsedTime < zoomInTime)
        {
            elapsedTime += Time.deltaTime;
            currentZoom = Mathf.Lerp(zoomOutTarget, zoomInTarget, elapsedTime / zoomInTime);
            Vector3 smoothedOffset = transform.forward * -currentZoom;
            transform.position = target.position + smoothedOffset;

            yield return null;
        }

        transform.rotation = targetRotation;
        transform.position = target.position + (targetRotation * Vector3.forward * -currentZoom);
        currentRotationY = transform.eulerAngles.y;
        currentRotationX = transform.eulerAngles.x;
        currentIslandIndex = newIndex;

        isCycling = false;
        canvasObj.SetActive(true);
    }
}