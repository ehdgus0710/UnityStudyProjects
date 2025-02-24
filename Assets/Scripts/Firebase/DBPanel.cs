using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DBPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField idInputField;
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private TMP_InputField ageInputField;
    [SerializeField]
    private TMP_InputField emailInputField;
    [SerializeField]
    private TMP_InputField locationInputField;

    [SerializeField]    
    private Button insertButton;
    [SerializeField]
    private Button updateButton;
    [SerializeField]
    private Button deleteButton;
    [SerializeField]
    private Button searchButton;

    private void Awake()
    {
        insertButton.onClick.AddListener(OnInsertButtonClicked);
        updateButton.onClick.AddListener(OnUpdateInfoButtonClicked);
        deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        searchButton.onClick.AddListener(OnSearchButtonClicked);
    }

    public async void OnInsertButtonClicked()
    {
        UserInfo info = CreateUserInfo();
        int.TryParse(idInputField.text, out int id);
        await UserInfoDAC.Instance.Insert(id, info);
    }

    private UserInfo CreateUserInfo()
    {
        string name = nameInputField.text;
        int.TryParse(ageInputField.text, out int age);
        string email = emailInputField.text;
        string location = locationInputField.text;
        
        return new UserInfo(name, age, email, location);
    }

    public async void OnUpdateInfoButtonClicked()
    {
        UserInfo info = CreateUserInfo();
        int.TryParse(idInputField.text, out int id);
        await UserInfoDAC.Instance.UpdateInfo(id, info);
    }
    public async void OnDeleteButtonClicked()
    {
        int.TryParse(idInputField.text, out int id);
        await UserInfoDAC.Instance.DeleteInfo(id);
    }

    public async void OnSearchButtonClicked()
    {
        await UserInfoDAC.Instance.Search();
    }

    private void OnStartEvent()
    {
        insertButton.interactable = false;
        updateButton.interactable = false;
        deleteButton.interactable = false;
        searchButton.interactable = false;

        idInputField.interactable = false;
        nameInputField.interactable = false;
        ageInputField.interactable = false;
        emailInputField.interactable = false;
        locationInputField.interactable = false;
    }

    private void OnEndEvent()
    {
        insertButton.interactable = true;
        updateButton.interactable = true;
        deleteButton.interactable = true;
        searchButton.interactable = true;

        idInputField.interactable = true;
        nameInputField.interactable = true;
        ageInputField.interactable = true;
        emailInputField.interactable = true;
        locationInputField.interactable = true;
    }
}
