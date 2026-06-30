using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StickerUpdate : DefaultInteract
{
    public int orderID;

    public TMP_Text redText;
    public TMP_Text greenText;
    public TMP_Text blackText;

    public GameObject blanckSprite;

    public Order? currentOrder;
    public void UpdateSticker()
    {
        var currentOrder = OrderManager.Instance.GetOrder(orderID);
        if (currentOrder != null)
        {
            greenText.text = currentOrder.Value.greenTeaRequired.ToString();
            redText.text = currentOrder.Value.redTeaRequired.ToString();
            blackText.text = currentOrder.Value.blackTeaRequired.ToString();
        }
        else
        {
            blanckSprite.SetActive(true);
        }
    }
}
