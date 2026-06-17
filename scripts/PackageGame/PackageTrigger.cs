using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tea")
        {
            switch (collision.gameObject.GetComponent<ItemPhysics>().itemType)
            {
                case ItemPhysics.ItemType.Black:
                    Debug.Log("Black");
                    break;
                case ItemPhysics.ItemType.Red:
                    Debug.Log("Red");
                    break;
                case ItemPhysics.ItemType.Green:
                    Debug.Log("Green");
                    break;
            }
            Destroy(collision.gameObject);
        }
    }
}
