using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private ItemList item;
    private Image image;
    private RectTransform rect;
    [SerializeField] private DragSlot drag;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        drag = GetComponentInChildren<DragSlot>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //eventData.pointerDrag : 현재 드래그 중인 오브젝트
            //eventData.pointerCurrentRaycast : 드리그중인 마우스 앞에 놓여진 아무 오브젝트
            if (!eventData.pointerCurrentRaycast.gameObject.CompareTag("Item"))
            {
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            }
            else
            {
                //Debug.Log("Swap");
                //Debug.Log("Drag : " + eventData.pointerDrag.GetComponent<ItemSlot>().item);
                //Debug.Log("CurrentRay : " + eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>().item);
                Swap(eventData.pointerDrag.GetComponent<ItemSlot>(), eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>());
                //Debug.Log("SwapDrag : " + eventData.pointerDrag.GetComponent<ItemSlot>().item);
                //Debug.Log("SwapCurrentRay : " + eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>().item);
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
