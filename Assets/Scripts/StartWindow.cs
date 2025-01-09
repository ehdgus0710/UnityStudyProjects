using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
    public bool canContinue = true;

    public Button continueButton;
    public Button newGameButton;
    public Button optionButton;


    protected override void Awake()
    {
        continueButton.onClick.AddListener(OnClickContinue);
        newGameButton.onClick.AddListener(OnClickNewGame);
        optionButton.onClick.AddListener(OnClickOption);

    }
    public override void Open()
    {
        continueButton.gameObject.SetActive(canContinue);

        if(continueButton.gameObject.activeSelf)
        {
            firstSelected = continueButton.gameObject;
        }

        base.Open();
    }

    public void OnClickContinue()
    {
    }

    public void OnClickNewGame()
    {
        windowManager.Open(Windows.Keyboard);
    }

    public void OnClickOption()
    {
        Debug.Log("2");
    }
}
