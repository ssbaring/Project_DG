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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("UIÅ¬¸¯");
        beginPoint = UIWindow.position;
        moveBegin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIWindow.position = beginPoint + (eventData.position - moveBegin);
    }
}
