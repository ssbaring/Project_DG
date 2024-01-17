using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler, IPointerClickHandler
{

    private Image image;
    private RectTransform rect;
    private PointerEventData ped;
    //[SerializeField] private DragSlot drag;
    [SerializeField] private RectTransform equipment;
    public bool isInventory = false;
    public ItemList.ItemType itemSlotType;


    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        equipment = transform.root.GetChild(0).GetChild(1).GetComponent<RectTransform>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
/*
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            foreach(RectTransform child in equipment.GetComponentsInChildren<RectTransform>())
            {
                if(child.childCount == 0 && (child.GetComponent<DropSlot>().itemSlotType & eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>().item.itemType) != 0)
                {
                    Debug.Log(child.name + "아이템 없음");
                    eventData.pointerDrag.transform.SetParent(child);
                    eventData.pointerDrag.GetComponent<RectTransform>().position = child.rect.position;
                }
            }
        }
*/
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //eventData.pointerDrag : 현재 드래그 중인 오브젝트
            //eventData.pointerCurrentRaycast : 드래그중인 마우스 앞에 놓여진 아무 오브젝트
            if (!eventData.pointerCurrentRaycast.gameObject.CompareTag("Item"))
            {
                if ((itemSlotType & eventData.pointerDrag.GetComponent<ItemSlot>().item.itemType) != 0)
                {
                    eventData.pointerDrag.transform.SetParent(transform);
                    eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

                    //장비 인벤토리가 아닐 시 예외처리
                    if (!isInventory)
                    {
                        eventData.pointerDrag.GetComponent<ItemSlot>().isEquipped = true;
                    }
                    else
                    {
                        eventData.pointerDrag.GetComponent<ItemSlot>().isEquipped = false;
                    }
                }
            }
            else
            {
                //둘 다 장착상태가 아님 또는 두 아이템의 타입이 같으면
                if ((!eventData.pointerDrag.GetComponent<ItemSlot>().isEquipped &&
                    !eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>().isEquipped) ||
                    (eventData.pointerDrag.GetComponent<ItemSlot>().item.itemType &
                    eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>().item.itemType) != 0)
                {
                    Swap(eventData.pointerDrag.GetComponent<ItemSlot>(), eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>());
                }
            }
        }
    }

    public void Swap(ItemSlot current, ItemSlot target)
    {
        ItemList tempItem = target.item;
        target.item = current.item;
        current.item = tempItem;
    }


}
