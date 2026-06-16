using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultInteract : MonoBehaviour
{
    [Header("Interaction")]
    public Sprite interactionIcon;
    public bool canInteract = true;

    [Header("Locking")]
    public bool isLocked = false;
    // --- Right ---
    public virtual void Interact()
    {
    }

    // Mouse Events
    public virtual void MouseDown()
    {
    }
    public virtual void MouseUp()
    {
    }
    public virtual void MouseProcess()
    {
    }

    // --- Left ---
    public virtual void RightInteract()
    {
    }

    // Mouse Events
    public virtual void RightMouseDown()
    {
    }
    public virtual void RightMouseUp()
    {
    }
    public virtual void RightMouseProcess()
    {
    }

    // --- Focus Events ---
    public virtual void onFocus()
    {
    }
    public virtual void onDefocus()
    {
    }
}
