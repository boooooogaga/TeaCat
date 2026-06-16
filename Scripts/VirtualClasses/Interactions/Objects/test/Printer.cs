using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : DefaultInteract
{
    public override void Interact()
    {
        Debug.Log("printing...");
    }
    public override void MouseDown()
    {
        Debug.Log("mouse down");
    }
    public override void MouseUp()
    {
        Debug.Log("mouse up");
    }
    public override void MouseProcess()
    {
        Debug.Log("process...");
    }

    public override void onFocus()
    {
        Debug.Log("focused");
    }
    public override void onDefocus()
    {
        Debug.Log("defocused");
    }
}