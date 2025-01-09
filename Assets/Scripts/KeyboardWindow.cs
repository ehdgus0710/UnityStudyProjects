using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardWindow : GenericWindow
{
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private GameObject keyboardButtons;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button acceptButton;

    [SerializeField] private float actionTime;
    [SerializeField] private float inputTime = 0.5f;
    private readonly string noneInputString = "_";
    private string inputString;
    private float currentInputTime = 0f;
    private bool isInput = false;
    private Coroutine inputNoneAction;

    protected override void Awake()
    {
        var buttons = keyboardButtons.GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => 
            { 
                inputString += button.GetComponentInChildren<TextMeshProUGUI>()?.text; 
                inputText.text = inputString;
                OnResetInput();
            });
        }

        clearButton.onClick.AddListener(OnClearText);
        deleteButton.onClick.AddListener(OnDeleteText);
        acceptButton.onClick.AddListener(OnAccept);
    }

    public override void Open()
    {
        base.Open();

        inputNoneAction = StartCoroutine(CoInputNoneAction());
        isInput = false;
    }

    public override void Close()
    {
        base.Close();
    }

    private void Update()
    {
        currentInputTime += Time.deltaTime;

        if(isInput && inputTime < currentInputTime)
        {
            isInput = false;
            inputNoneAction = StartCoroutine(CoInputNoneAction());
        }
    }

    private void OnResetInput()
    {
        currentInputTime = 0f;
        isInput = true;
    }

    private void OnDeleteText()
    {
        int count = inputString.Length;

        if (count == 0)
            return;

        inputString = inputString.Remove(count - 1, 1);
        inputText.text = inputString;
        OnResetInput();
    }

    private void OnClearText()
    {
        inputString = "";
        inputText.text = inputString;

        OnResetInput();
    }

    private void OnAccept()
    {
        windowManager.Open(Windows.GameOver);
    }

    private IEnumerator CoInputNoneAction()
    {
        bool isDraw = true;
        while (!isInput)
        {
            yield return new WaitForSeconds(actionTime);

            isDraw = !isDraw;
            if (isDraw)
                inputText.text = inputString;
            else
                inputText.text = $"{inputString}{noneInputString}";
        }
    }
}
