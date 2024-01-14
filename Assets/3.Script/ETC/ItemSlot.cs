using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public bool isItemUseSlot = false;

    private Image itemImage;
    [SerializeField] private List<ItemList> itemLists;

    public List<ItemList> ItemLists
    {
        get
        {
            return itemLists;
        }
        set
        {
            itemLists = value;
            if (isItemUseSlot)
            {
                
            }
        }
    }
}
