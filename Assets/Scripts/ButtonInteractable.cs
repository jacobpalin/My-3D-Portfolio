using UnityEngine;

public class ButtonInteractable : Interactable
{
    public override void OnInteract()
    {
        return;
    }

    public override void OnHover()
    {
        base.OnHover();
    }

    public override void OnHoverExit()
    {
        base.OnHoverExit();
    }
}