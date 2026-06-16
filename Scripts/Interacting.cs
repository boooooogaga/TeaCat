using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    public enum InteractionMode
    {
        Mode2D,
        Mode3D
    }

    [Header("Interaction Settings")]
    public GameObject Target;
    public DefaultInteract InteractiveTarget;
    public bool canInteract = true;

    [Header("Mode")]
    [SerializeField] InteractionMode interactionMode = InteractionMode.Mode2D;

    [SerializeField] float rayDistance = 100f;
    [SerializeField] LayerMask interactionMask = ~0;

    [Header("Visual")]
    [SerializeField] Material outline;

    HashSet<Renderer> outlined = new HashSet<Renderer>();

    [Header("Locking")]
    public GameObject localLock;

    [Header("Progress")]
    public bool progressed;
    public float interactionProgress;

    void Update()
    {
        if (!canInteract)
        {
            ResetInteract();
            return;
        }

        GameObject MouseTarget = null;

        switch (interactionMode)
        {
            case InteractionMode.Mode2D:
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Collider2D hit = Physics2D.OverlapPoint(mousePos);

                    if (hit != null)
                        MouseTarget = hit.gameObject;

                    break;
                }

            case InteractionMode.Mode3D:
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactionMask))
                        MouseTarget = hit.collider.gameObject;

                    break;
                }
        }

        if (localLock != null)
        {
            MouseTarget = localLock;
        }

        if (MouseTarget != Target)
        {
            if (Target != null)
                HoverOff();

            if (MouseTarget != null)
                HoverOn(MouseTarget);
        }

        if (InteractiveTarget != null)
        {
            // left
            if (Input.GetMouseButtonDown(0))
            {
                MouseDown();
                MouseClick();
            }
            else if (Input.GetMouseButtonUp(0))
                MouseUp();
            if (Input.GetMouseButton(0) && InteractiveTarget.canInteract)
                InteractiveTarget.MouseProcess();

            // right
            if (Input.GetMouseButtonDown(1))
            {
                MouseDown(true);
                MouseClick(true);
            }
            else if (Input.GetMouseButtonUp(1))
                MouseUp(true);
            if (Input.GetMouseButton(1) && InteractiveTarget.canInteract)
                InteractiveTarget.RightMouseProcess();

            if (InteractiveTarget is ProgressInteract prog && !prog.inProgress && localLock == InteractiveTarget.gameObject && InteractiveTarget.isLocked == false)
                localLock = null;
        }
    }


    void ResetInteract()
    {
        if (Target != null)
        {
            HoverOff();
            if (localLock == null)
                RemoveOutline(Target);
        }
    }

    // Outline
    void SetOutline(GameObject obj)
    {
        if (obj == null) return;
        var rend = obj.GetComponent<Renderer>();
        if (!rend || outlined.Contains(rend)) return;

        var mats = new List<Material>(rend.materials);
        mats.Add(outline);
        rend.materials = mats.ToArray();

        outlined.Add(rend);
    }

    void RemoveOutline(GameObject obj)
    {
        if (obj == null) return;
        var rend = obj.GetComponent<Renderer>();
        if (!rend) return;

        var mats = new List<Material>(rend.materials);
        mats.RemoveAll(m => m.shader == outline.shader);
        rend.materials = mats.ToArray();

        outlined.Remove(rend);
    }

    // Hover
    void HoverOff()
    {
        if (Target != null && InteractiveTarget != null)
        {
            if (Target != localLock)
                RemoveOutline(Target);

            InteractiveTarget.onDefocus();
            Target = null;
            InteractiveTarget = null;
        }
    }

    void HoverOn(GameObject MouseTarget)
    {
        if (MouseTarget.TryGetComponent<DefaultInteract>(out var inter))
        {
            Target = MouseTarget;
            InteractiveTarget = inter;

            SetOutline(Target);
            inter.onFocus();
            InteractiveTarget.interactionIcon = inter.interactionIcon;
        }
    }

    // Click
    void MouseClick(bool rside = false)
    {
        if (InteractiveTarget != null && InteractiveTarget.canInteract)
        {
            if (rside)
                InteractiveTarget.RightInteract();
            else
                InteractiveTarget.Interact();
        }
    }

    void MouseDown(bool rside = false)
    {
        if (InteractiveTarget != null && InteractiveTarget.canInteract)
        {
            if (rside)
                InteractiveTarget.RightMouseDown();
            else
                InteractiveTarget.MouseDown();
        }
    }

    void MouseUp(bool rside = false)
    {
        if (InteractiveTarget != null && InteractiveTarget.canInteract)
        {
            if (rside)
                InteractiveTarget.RightMouseUp();
            else
                InteractiveTarget.MouseUp();
        }
    }
}
