using Firebase;
using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpUI : MonoBehaviour
{
    public AuthPanel authPanel;
    public TMP_InputField nameField;
    public TMP_InputField confirmPasswordField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;

    public Button signUpButton;
    public Button cancelButton;
    public TextMeshProUGUI alertText;

    private void Start()
    {
        signUpButton.onClick.RemoveListener(OnSignUpButtonClicked);
        signUpButton.onClick.AddListener(OnSignUpButtonClicked);
        cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnEnable()
    {
        ResetStatus();
    }

    private async void OnSignUpButtonClicked()
    {
        if (passwordField.text != confirmPasswordField.text)
        {
            alertText.gameObject.SetActive(true);
            alertText.text = "Passwords do not match!";
            return;
        }
        if (nameField.text.Length < 1)
        {

            alertText.gameObject.SetActive(true);
            alertText.text = "Please Enter a Name!";
            return;
        }

        signUpButton.interactable = false;
        cancelButton.interactable = false;

        try
        {
            await FirebaseAuthManager.Instance.CreateUserAsync(emailField.text, passwordField.text);
            UserProfile profile = new UserProfile { DisplayName = nameField.text};

            await FirebaseAuthManager.Instance.ChangeUserProfile(profile);
            FirebaseAuthManager.Instance.Logout();

            ResetStatus();
            authPanel.OpenLoginPanel();

        }
        catch (FirebaseException e)
        {
            alertText.text = FirebaseAuthManager.Instance.GetErrorMessage(e);
            alertText.gameObject.SetActive(true);
        }
        finally
        {
            signUpButton.interactable = true;
            cancelButton.interactable = true;
        }
    }

    private void OnCancelButtonClicked()
    {
        ResetStatus();
        authPanel.OpenLoginPanel();
    }

    private void ResetStatus()
    {
        alertText.text = string.Empty;
        nameField.text = string.Empty;
        confirmPasswordField.text = string.Empty;
        emailField.text = string.Empty;
        passwordField.text = string.Empty;

        alertText.gameObject.SetActive(false);
    }
}
