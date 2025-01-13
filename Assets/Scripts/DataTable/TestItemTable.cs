using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemTable : MonoBehaviour
{
    void Start()
    {
        var itemTable = DataTableManager.ItemTable;

        Debug.Log(itemTable.Get("Item1").ToString());
    }
}
