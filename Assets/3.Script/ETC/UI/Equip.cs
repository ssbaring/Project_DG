using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public Transform equipment;

    public bool isEquip;

    private void Update()
    {
        if(transform.childCount == 0)
        {
            isEquip = false;
        }

        if(isEquip)
        {
            transform.GetComponentInChildren<ItemSlot>().isEquipped = true;
        }
    }
}
