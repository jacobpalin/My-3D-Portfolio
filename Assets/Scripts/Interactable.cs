using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public virtual void OnInteract()
    {
        Debug.Log($"{gameObject.name} was interacted with!");
    }

    public virtual void OnHover()
    {
        // Default hover behavior, override in derived classes if needed
        if (InteractableClickHandler.HoverObject != null)
        {
            GameObject hoverObj = InteractableClickHandler.HoverObject;
            hoverObj.transform.SetParent(transform);
            hoverObj.transform.localScale = Vector3.one;

            // Check if it's a UI button (using RectTransform)
            if (hoverObj.transform.parent.GetComponent<RectTransform>())
            {
                // This is a UI object, so we handle it differently
                hoverObj.transform.localPosition = new Vector3(0, 0, -10); // Adjust the height
                hoverObj.transform.localRotation = Quaternion.Euler(90, 0, 0); // UI objects don't rotate
            }
            else
            {
                // This is a world-space object, so we apply rotation
                hoverObj.transform.localPosition = new Vector3(0, 1, 0); // Adjust the height
                hoverObj.transform.localRotation = transform.rotation; // Match the interactable's rotation
            }
        }
    }

    public virtual void OnHoverExit()
    {
        // Default hover exit behavior, override in derived classes if needed
        if (InteractableClickHandler.HoverObject != null)
        {
            GameObject hoverObj = InteractableClickHandler.HoverObject;
            hoverObj.transform.SetParent(null);
            hoverObj.transform.position = Vector3.zero;
            hoverObj.transform.rotation = Quaternion.identity;
            hoverObj.transform.localScale = Vector3.one;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
}