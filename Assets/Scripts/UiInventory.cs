using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UiInventory : MonoBehaviour
{
    public UiItemSlot prefabSlot;
    public ScrollRect scrollRect;

    public List<UiItemSlot> slots;

    public int maxSlot = 30;

    private void Awake()
    {
        for (int i = 0; i < maxSlot; ++i)
        {
            var slot = Instantiate(prefabSlot, scrollRect.content);
            slot.SetEmpty();
            slots.Add(slot);
        }
    }

    private void OnEnable()
    {
        UpdateSlots(SaveLoadManager.Data.items);
    }

    private void UpdateSlots(List<SaveItemData> items)
    {
        for(int i = 0; i < maxSlot; ++i)
        {
            if(i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void OnClickAdd()
    {
        var instance = new SaveItemData();
        var itemId = $"Item{Random.Range(1, 5)}";
        instance.data = DataTableManager.ItemTable.Get(itemId);
        SaveLoadManager.Data.items.Add(instance);

        // 삭제 예정
        SaveLoadManager.Save();

        UpdateSlots(SaveLoadManager.Data.items);
    }
}
