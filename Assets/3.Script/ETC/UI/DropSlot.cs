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
                    //��� ������� && ��� ������ Ÿ�԰� ��Ŭ�� �� �������� Ÿ���� ����
                    if (!equipmentChild.isEquip && (equipmentChild.GetComponent<DropSlot>().itemSlotType & eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().item.itemType) != 0)
                    {
                        Debug.Log("�ٷ� ����");
                        eventData.pointerPressRaycast.gameObject.transform.SetParent(equipmentChild.gameObject.transform);
                        eventData.pointerPressRaycast.gameObject.GetComponent<RectTransform>().position = equipmentChild.GetComponent<RectTransform>().position;
                        break;
                    }
                    //��� �����Ǿ�����
                    else if (equipmentChild.isEquip)
                    {
                        //����� �ڽĿ�����Ʈ�� ���� ��
                        if (equipmentChild.gameObject.GetComponentInChildren<ItemSlot>() != null)
                        {
                            //��� ���� �ڽ�������Ʈ�� ������ Ÿ�԰� ��ư ���� ������Ʈ�� ������Ʈ ������ Ÿ���� ���� ��
                            if ((equipmentChild.gameObject.transform.GetComponentInChildren<ItemSlot>().item.itemType
                                & eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().item.itemType) != 0 &&
                                isInventory)
                            {
                                Debug.Log("��� ��ü");
                                Swap(eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>(), equipmentChild.gameObject.transform.GetComponentInChildren<ItemSlot>());
                                break;
                            }
                            else if (eventData.pointerPressRaycast.gameObject.GetComponent<ItemSlot>().isEquipped)
                            {
                                Debug.Log("���� ����");
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
            //eventData.pointerDrag : ���� �巡�� ���� ������Ʈ
            //eventData.pointerCurrentRaycast : �巡������ ���콺 �տ� ������ �ƹ� ������Ʈ
            if (!eventData.pointerCurrentRaycast.gameObject.CompareTag("Item"))
            {
                if ((itemSlotType & eventData.pointerDrag.GetComponent<ItemSlot>().item.itemType) != 0)
                {
                    eventData.pointerDrag.transform.SetParent(transform);
                    eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;

                    //��� �κ��丮�� �ƴ� �� ����ó��
                    if (isInventory)
                    {
                        eventData.pointerDrag.GetComponent<ItemSlot>().isEquipped = false;
                    }
                }
            }
            else
            {
                //�� �� �������°� �ƴ� �Ǵ� �� �������� Ÿ���� ������
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
