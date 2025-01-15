using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour, IDragHandler
{
    public UiItemSlot prefabSlot;
    public ScrollRect scrollRect;

    public List<UiItemSlot> slotList;
    public int SeletedSlotIndex { get; private set; } = -1;
    public int maxSlot = 30;

    public UnityEvent onClickAddButton;
    public UnityEvent onClickRemoveButton;
    public UnityEvent onUpdateSlots;


    private List<SaveItemData> tempItemList;
    private List<SaveItemData> filterList;
    [SerializeField] private TMP_Dropdown itemFilter;
    private int sortingIndex = -1;
    private int filterIndex = -1;
    private int filterDefaultOptionCount;

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in slotList)
        {
            slot.button.onClick.AddListener(action);
        }
    }


    public List<UiItemSlot> testList;
    private void Awake()
    {
        for (int i = 0; i < maxSlot; ++i)
        {
            var slot = Instantiate(prefabSlot, scrollRect.content);
            slot.SlotIndex = i;
            slot.SetEmpty();
            slot.button.onClick.AddListener(() => { SeletedSlotIndex = slot.SlotIndex; });
            slot.onPointerEnter.AddListener(() =>
            {
                if (!testList.Contains(slot))
                    testList.Add(slot);
            });

            // slot.onPointerExit.AddListener(() => testList.Remove(slot));
            slotList.Add(slot);
        }

        filterDefaultOptionCount = itemFilter.options.Count;
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < (int)ItemTypes.End; ++i)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = ((ItemTypes)i).ToString();
            optionDatas.Add(optionData);
        }

        itemFilter.AddOptions(optionDatas);
    }

    private void OnEnable()
    {
        tempItemList = SaveLoadManager.Data.itemList.ToList();
        UpdateSlots(tempItemList);
    }

    private void OnDisable()
    {
        SaveLoadManager.Data.itemList = tempItemList.ToList();
        SaveLoadManager.Save();
    }

    private void UpdateSlots(List<SaveItemData> items)
    {
        for(int i = 0; i < maxSlot; ++i)
        {
            if(i < items.Count)
            {
                slotList[i].SetItem(items[i]);
            }
            else
            {
                slotList[i].SetEmpty();
            }
        }

        SeletedSlotIndex = -1;
        onUpdateSlots?.Invoke();
    }

    public void OnClickAdd()
    {
        var instance = new SaveItemData();
        var itemId = $"Item{Random.Range(1, 5)}";
        instance.data = DataTableManager.ItemTable.Get(itemId);
        tempItemList.Add(instance);
        onClickAddButton?.Invoke();

        if(filterIndex == -1)
            UpdateSlots(tempItemList);
        else
        {
            OnFilterChangedSorting(filterIndex);
        }
    }

    public void OnClickRemove()
    {
        if (SeletedSlotIndex == -1)
            return;

        // Data에 지워주는 메소드를 제작해야 함.
        // 해당 메소드에서 유효성 검사를 진행
        tempItemList.Remove(slotList[SeletedSlotIndex].SaveItemData);

        if (filterIndex == -1)
            UpdateSlots(tempItemList);
        else
        {
            OnFilterChangedSorting(filterIndex);
        }
        // UpdateSlots(tempItemList);
        onClickRemoveButton?.Invoke();
    }


    public enum SortingOptions
    {
        // CustomOrder,
        NameAcceding,
        NameDeccending,
        CostAccending,
        CostDeccending
    }

    private readonly System.Comparison<SaveItemData>[] comparisons =
    {
        ((lhs,rhs) => lhs.data.StringName.CompareTo(rhs.data.StringName)),
        ((lhs, rhs) => rhs.data.StringName.CompareTo(lhs.data.StringName)),
        ((lhs, rhs) => lhs.data.Cost.CompareTo(rhs.data.Cost)),
        ((lhs, rhs) => rhs.data.Cost.CompareTo(lhs.data.Cost))
    };

    public void OnValueChangedSorting(int index)
    {
        sortingIndex = index;
        tempItemList.Sort(comparisons[sortingIndex]);

        UpdateSlots(tempItemList);
    }

    public void OnFilterChangedSorting(int index)
    {
        filterIndex = index;
        if(filterIndex == 0)
        {
            filterIndex = -1;
            UpdateSlots(tempItemList);
            return;
        }

        ItemTypes currentItem = (ItemTypes)(index - filterDefaultOptionCount);
        filterList = tempItemList.Where((data) => data.data.Type == currentItem).ToList();

        if(sortingIndex != -1)
            filterList.Sort(comparisons[sortingIndex]);

        UpdateSlots(filterList);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach(var item in results)
        {
            Debug.Log(item.gameObject.name);
        }
    }
}
