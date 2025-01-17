using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableClickHandler : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;

    private Interactable currentHoveredInteractable;

    public static GameObject HoverObject;
    [SerializeField] private GameObject hoverObject;

    private void Awake()
    {
        if (hoverObject != null)
        {
            HoverObject = hoverObject;
        }
    }

    void Update()
    {
        HandleMouseHover();

        if (IsPointerOverUI()) return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleWorldspaceClick();
        }
    }

    private void HandleMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (currentHoveredInteractable != interactable)
                {
                    ResetHover();
                    currentHoveredInteractable = interactable;
                    currentHoveredInteractable.OnHover();
                }
                return;
            }
        }

        ResetHover();
    }

    private void ResetHover()
    {
        if (currentHoveredInteractable != null)
        {
            currentHoveredInteractable.OnHoverExit();
            currentHoveredInteractable = null;
        }
    }

    private void HandleWorldspaceClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}