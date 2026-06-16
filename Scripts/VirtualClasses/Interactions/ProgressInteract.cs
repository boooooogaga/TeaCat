using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInteract : DefaultInteract
{
    [Header("Progress Values")]
    public bool progressed;
    public bool inProgress;

    [Header("Progress Settings")]
    public float progressDuration = 1f;
    public float interactionProgress = 0f;

    public override void RightMouseDown()
    {
        inProgress = true;
        progressed = false;
        interactionProgress = 0f;
    }

    public override void RightMouseProcess()
    {
        if (inProgress && !progressed)
        {
            interactionProgress += Time.deltaTime / progressDuration;
            if (interactionProgress >= 1f)
            {
                interactionProgress = 1f;
                inProgress = false;
                progressed = true;
                ProgressedInteract();
            }
        }
    }

    public void ProgressReset()
    {
        inProgress = false;
        interactionProgress = 0f;
        progressed = false;
    }
    public override void RightMouseUp()
    {
        ProgressReset();
    }
    public override void onDefocus()
    {
        ProgressReset();
    }

    public virtual void ProgressedInteract()
    {
    }
}
