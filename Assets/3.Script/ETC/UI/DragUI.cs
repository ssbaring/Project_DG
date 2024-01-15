using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform UIWindow;

    private Vector2 beginPoint;
    private Vector2 moveBegin;

    private void Awake()
    {
        if(UIWindow == null)
        {
            UIWindow = transform.parent;
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        beginPoint = UIWindow.position;
        moveBegin = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        UIWindow.position = beginPoint + (eventData.position - moveBegin);
    }
}
