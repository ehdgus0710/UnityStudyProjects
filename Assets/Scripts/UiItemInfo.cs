using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    public Image            itemIcon;
    public TextMeshProUGUI  itemName;
    public TextMeshProUGUI  itemType;
    public TextMeshProUGUI  itemValue;
    public TextMeshProUGUI  itemCost;
    public TextMeshProUGUI  itemDesc;

    public SaveItemData SaveItemData { get; private set; }

    public void SetEmpty()
    {
        SaveItemData = null;

        itemIcon.sprite = null;
        itemName.text = string.Empty;
        itemType.text = string.Empty;
        itemValue.text = string.Empty;
        itemCost.text = string.Empty;
        itemDesc.text = string.Empty;
    }

    public void SetData(SaveItemData saveItemData)
    {
        SaveItemData = saveItemData;

        itemIcon.sprite = saveItemData.data.IconSprite;
        itemIcon.type = Image.Type.Simple;
        itemIcon.preserveAspect = true;
        itemName.text = saveItemData.data.StringName;
        itemType.text = saveItemData.data.Type.ToString();
        itemValue.text = saveItemData.data.Value.ToString();
        itemCost.text = saveItemData.data.Cost.ToString();
        itemDesc.text = saveItemData.data.StringDesc;
    }
}
