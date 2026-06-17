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

    [Header("3D Outline")]
    HashSet<Renderer> outlined3D = new();
    Dictionary<GameObject, List<GameObject>> outlined2D = new();

    [Header("2D Outline")]
    [SerializeField] float outlineScale = 0.05f;
    [SerializeField] Color outlineColor = Color.white;


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
        switch (interactionMode)
        {
            case InteractionMode.Mode2D:
                SetOutline2D(obj);
                break;

            case InteractionMode.Mode3D:
                SetOutline3D(obj);
                break;
        }
    }

    void RemoveOutline(GameObject obj)
    {
        switch (interactionMode)
        {
            case InteractionMode.Mode2D:
                RemoveOutline2D(obj);
                break;

            case InteractionMode.Mode3D:
                RemoveOutline3D(obj);
                break;
        }
    }

    // 3D
    void SetOutline3D(GameObject obj)
    {
        if (obj == null) return;

        foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
        {
            if (outlined3D.Contains(rend))
                continue;

            List<Material> mats = new(rend.materials);
            mats.Add(outline);

            rend.materials = mats.ToArray();

            outlined3D.Add(rend);
        }
    }

    void RemoveOutline3D(GameObject obj)
    {
        if (obj == null) return;

        foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
        {
            List<Material> mats = new(rend.materials);

            mats.RemoveAll(m => m.shader == outline.shader);

            rend.materials = mats.ToArray();

            outlined3D.Remove(rend);
        }
    }

    // 2D
    void SetOutline2D(GameObject obj)
    {
        if (obj == null)
            return;

        if (outlined2D.ContainsKey(obj))
            return;

        SpriteRenderer source = obj.GetComponent<SpriteRenderer>();

        if (source == null)
            return;

        List<GameObject> clones = new();

        GameObject outer = new("Outline Outer");
        GameObject inner = new("Outline Inner");

        outer.transform.SetParent(obj.transform, false);
        inner.transform.SetParent(obj.transform, false);

        SpriteRenderer outerSR = outer.AddComponent<SpriteRenderer>();
        SpriteRenderer innerSR = inner.AddComponent<SpriteRenderer>();

        outerSR.sprite = source.sprite;
        innerSR.sprite = source.sprite;

        outerSR.color = outlineColor;
        innerSR.color = outlineColor;

        outerSR.material = outline;
        innerSR.material = outline;

        outerSR.sortingLayerID = source.sortingLayerID;
        innerSR.sortingLayerID = source.sortingLayerID;

        outerSR.sortingOrder = source.sortingOrder - 1;
        innerSR.sortingOrder = source.sortingOrder - 1;

        outer.transform.localScale =
            Vector3.one * (1f + outlineScale);

        inner.transform.localScale =
            Vector3.one * (1f - outlineScale);

        clones.Add(outer);
        clones.Add(inner);

        outlined2D.Add(obj, clones);
    }
    void RemoveOutline2D(GameObject obj)
    {
        if (!outlined2D.TryGetValue(obj, out List<GameObject> clones))
            return;

        foreach (GameObject clone in clones)
        {
            if (clone != null)
                Destroy(clone);
        }

        outlined2D.Remove(obj);
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
