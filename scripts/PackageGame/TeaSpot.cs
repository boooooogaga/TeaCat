using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaSpot : DefaultInteract
{
    CursorControll cursor;
    public GameObject tea;

    Vector3 SpawnPoint = Vector3.zero;
    void Start()
    {
        cursor = GameObject.Find("hand_0").GetComponent<CursorControll>();
    }

    public override void onFocus()
    {
        Debug.Log("Focus");
        cursor.SetStateSprite(2);
    }
    public override void onDefocus()
    {
        Debug.Log("Defocus");
        cursor.SetStateSprite(0);
    }
    public override void Interact()
    {
        Debug.Log("Interact");
        GameObject instantiatedTea = Instantiate(tea, SpawnPoint, Quaternion.identity);
        cursor.SetStateSprite(1);
    }
}
