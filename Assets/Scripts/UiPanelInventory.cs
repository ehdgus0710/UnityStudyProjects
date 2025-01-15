using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPanelInventory : MonoBehaviour
{
    public UiInventory inventory;
    public UiItemInfo itemInfo;

    private void Start()
    {
        inventory.AddListeners(OnClickInvenSlot);
        inventory.onClickAddButton.AddListener(OnClickAddButton);
        inventory.onClickRemoveButton.AddListener(OnClickRemoveItem);
    }

    private void OnClickInvenSlot()
    {
        int index = inventory.SeletedSlotIndex;
        if (index != -1 && inventory.slotList[index].SaveItemData != null)
        {
            itemInfo.SetData(inventory.slotList[index].SaveItemData);
        }
        else
        {
            itemInfo.SetEmpty();
        }
    }

    private void OnClickAddButton()
    {

    }
    private void OnClickRemoveItem()
    {
        itemInfo.SetEmpty();
    }
}
