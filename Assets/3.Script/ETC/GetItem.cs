using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    [SerializeField] private ItemList itemList;

    private ItemSlot slot;
    private bool isAbleGet = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isAbleGet = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isAbleGet = false;
    }

    private void Start()
    {
        slot = FindObjectOfType<ItemSlot>();
        itemList = transform.parent.GetComponent<OpenChest>().item;
    }

    private void Update()
    {
        if(isAbleGet && Input.GetKeyDown(GameManager.instance.InteractionKey))
        {
            slot.ItemLists.Add(itemList);
            Debug.Log(itemList);
        }
    }
}
