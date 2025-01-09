using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour
{
    public GameObject firstSelected;

    protected WindowManager windowManager;


    protected virtual void Awake()
    {
    }

    public void Init(WindowManager windowManager)
    {
        this.windowManager = windowManager;
    }

    public void OnFocus()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        OnFocus();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    
}
