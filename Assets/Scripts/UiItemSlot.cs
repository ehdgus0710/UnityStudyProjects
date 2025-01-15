using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// 마우스 포인터가 들어오고 나가는 인터페이스 IPointerEnterHandler, IPointerExitHandler
public class UiItemSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    , IPointerEnterHandler, IPointerExitHandler
{
    public SaveItemData SaveItemData { get; private set; }
    public int          SlotIndex { get; set; }

    [SerializeField] public Button button;
    public Image itemIcon;
    public TextMeshProUGUI itemName;

    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemName.text = string.Empty;
        SaveItemData = null;
    }

    public void SetItem(SaveItemData itemData)
    {
        SaveItemData = itemData;
        itemIcon.sprite = SaveItemData.data.IconSprite;
        itemIcon.SetNativeSize();
        itemName.text = SaveItemData.data.StringName;
    }

    public void OnClick()
    {
        Debug.Log($"Slot Index : {SlotIndex}");
        if(SaveItemData != null)
            Debug.Log($"Instance Id : {SaveItemData.instanceId} / Item Id : {SaveItemData.data.Id}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        var parent = transform.parent;
        Debug.Log($"OnDrag : {SlotIndex}");

        while(parent != null)
        {
            if (ExecuteEvents.CanHandleEvent<IDragHandler>(parent.gameObject))
            {
                ExecuteEvents.Execute(parent.gameObject, eventData, ExecuteEvents.dragHandler); ;
            }

            parent = parent.parent;
        }
     
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag : {SlotIndex}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag : {SlotIndex}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"PointerEnter : {SlotIndex}");
        onPointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"PointerExit : {SlotIndex}");
        onPointerExit?.Invoke();
    }
}
