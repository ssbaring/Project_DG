using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemImage;
    public bool isItemIn = false;

    public ItemList item;


    private void Update()
    {
        if (isItemIn)
        {
            itemImage.color = new Color(1, 1, 1, 1);
            itemImage.sprite = item.itemSprite;
        }
        else
        {
            itemImage.color = new Color(1, 1, 1, 0);
            item = null;
            itemImage = null;
        }
    }
}
