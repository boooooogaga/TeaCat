using UnityEngine;

public class FastDialogue : DefaultInteract
{
    [SerializeField] Dialogue dialogue;
    DialogeManager manager;
    Sprite iconsave = null;

    void Awake()
    {
        manager = FindObjectOfType<DialogeManager>();
        iconsave = this.interactionIcon;
    }

    void VisualUpdate(bool visible)
    {
        this.canInteract = visible;
        this.interactionIcon = visible ? iconsave : null;
    }

    public override void Interact()
    {
        if (manager != null)
        {
            manager.StartDialogue(dialogue);
            VisualUpdate(false);
        }
    }

    public override void onFocus()
    {
       VisualUpdate(!manager.inDialogue);
    }
}
