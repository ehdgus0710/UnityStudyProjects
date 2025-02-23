using Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public AuthPanel authPanel;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public Button signInButton;
    public Button signUpButton;
    public TextMeshProUGUI alertText;

    private void Start()
    {
        signUpButton.onClick.RemoveListener(OnSignUpButtonClicked);
        signUpButton.onClick.AddListener(OnSignUpButtonClicked);

        signInButton.onClick.RemoveListener(OnSignInButtonClicked);
        signInButton.onClick.AddListener(OnSignInButtonClicked);
    }

    public async void OnSignInButtonClicked()
    {
        signInButton.interactable = false;
        signUpButton.interactable = false;

        try
        {
            await FirebaseAuthManager.Instance.SignInUserAsync(emailField.text, passwordField.text);
            authPanel.OpenProfilePanel();
        }
        catch (FirebaseException e)
        {
            alertText.text = FirebaseAuthManager.Instance.GetErrorMessage(e);
            alertText.gameObject.SetActive(true);
        }
        finally
        {
            signInButton.interactable = true;
            signUpButton.interactable = true;
        }
    }

    public void OnSignUpButtonClicked()
    {
        authPanel.OpenSignUpPanel();
    }
}
