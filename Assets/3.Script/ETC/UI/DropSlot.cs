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
    [SerializeField] private RectTransform slot;
    public bool isInventory = false;
    public bool isSlotUse = false;


    [Header("Slot Type")]
    public ItemList.ItemType itemSlotType;


    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        equipment = transform.root.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        slot = transform.root.GetChild(0).GetChild(2).GetComponent<RectTransform>();
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

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>() != null)
            {
                foreach (Equip equipmentChild in equipment.GetComponentsInChildren<Equip>())
                {
                    //장비가 비어있음 && 장비 슬롯의 타입과 우클릭 한 아이템의 타입이 같음
                    if (!equipmentChild.isEquip && (equipmentChild.GetComponent<DropSlot>().itemSlotType & eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().item.itemType) != 0)
                    {
                        Debug.Log("바로 장착");
                        eventData.pointerPressRaycast.gameObject.transform.SetParent(equipmentChild.gameObject.transform);
                        eventData.pointerPressRaycast.gameObject.GetComponent<RectTransform>().position = equipmentChild.GetComponent<RectTransform>().position;
                        break;
                    }
                    //장비가 장착되어있음
                    else if (equipmentChild.isEquip)
                    {
                        //장비의 자식오브젝트가 있을 때
                        if (equipmentChild.gameObject.GetComponentInChildren<ItemSlot>() != null)
                        {
                            //장비 슬롯 자식컴포넌트의 아이템 타입과 버튼 누른 오브젝트의 컴포넌트 아이템 타입이 같을 시
                            if ((equipmentChild.gameObject.transform.GetComponentInChildren<ItemSlot>().item.itemType
                                & eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().item.itemType) != 0 &&
                                isInventory)
                            {
                                Debug.Log("장비 교체");
                                Swap(eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>(), equipmentChild.gameObject.transform.GetComponentInChildren<ItemSlot>());
                                break;
                            }
                            else if (eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().isEquipped)
                            {
                                Debug.Log("장착 해제");
                                foreach (DropSlot item in slot.GetComponentsInChildren<DropSlot>())
                                {
                                    if (!item.isSlotUse)
                                    {
                                        eventData.pointerPressRaycast.gameObject.transform.SetParent(item.gameObject.transform);
                                        eventData.pointerPressRaycast.gameObject.GetComponent<RectTransform>().position = item.GetComponent<RectTransform>().position;
                                        eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().isEquipped = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else if (eventData.pointerPressRaycast.gameObject.transform.childCount == 0) return;
        }

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
                    if (isInventory)
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

    private void Update()
    {
        if (transform.childCount != 0)
        {
            isSlotUse = true;
        }
        else
        {
            isSlotUse = false;
        }
    }
}
