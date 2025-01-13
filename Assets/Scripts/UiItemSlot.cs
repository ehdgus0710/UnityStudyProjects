using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemName.text = string.Empty;
    }

    public void SetItem(SaveItemData itemData)
    {
        itemIcon.sprite = itemData.data.IconSprite;
        itemName.text = itemData.data.StringName;
    }
}
