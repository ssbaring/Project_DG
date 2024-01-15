using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    public ItemList itemList;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private InventoryUI inventory;
    private bool isAbleGet = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isAbleGet = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isAbleGet = false;
    }

    private void Awake()
    {
        inventory = FindObjectOfType<UIManager>().transform.GetChild(0).GetComponent<InventoryUI>();
    }

    private void Start()
    {
        itemList = transform.parent.GetComponent<OpenChest>().item;
    }

    private void Update()
    {
        if (isAbleGet && Input.GetKeyDown(GameManager.instance.InteractionKey))
        {
            isAbleGet = false;
            for (int i = 0; i < inventory.itemInventoryPosition.Length; i++)
            {
                Instantiate(itemPrefab, inventory.itemInventoryPosition[i].transform);
                inventory.itemInventoryPosition[i].GetComponentInChildren<ItemSlot>().item = itemList;
                inventory.itemInventoryPosition[i].GetComponentInChildren<ItemSlot>().itemImage.color = new Color(1, 1, 1, 1);
                inventory.itemInventoryPosition[i].GetComponentInChildren<ItemSlot>().itemImage.sprite = itemList.itemSprite;
                inventory.itemInventoryPosition[i].GetComponentInChildren<ItemSlot>().isItemIn = true;
                if (inventory.itemInventoryPosition[i].GetComponentInChildren<ItemSlot>().isItemIn == false)
                {
                    break;
                }
            }
            Debug.Log(itemList);
            gameObject.SetActive(false);
        }
    }
}
