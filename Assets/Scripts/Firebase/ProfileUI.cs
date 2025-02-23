using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    public AuthPanel authPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI emailText;
    public Button logoutButton;

    private void Awake()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }


    private void OnEnable()
    {
        FirebaseUser user = FirebaseAuthManager.Instance.User;

        if(user != null)
        {
            nameText.text = user.DisplayName;
            emailText.text = user.Email;
        }
        else
        {
            nameText.text = string.Empty;
            emailText.text = string.Empty;
        }

    }

    private void OnLogoutButtonClicked()
    {
        FirebaseAuthManager.Instance.Logout();
        authPanel.OpenLoginPanel();
    }
}
