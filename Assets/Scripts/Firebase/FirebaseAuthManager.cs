using Firebase;
using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseAuthManager : Singleton<FirebaseAuthManager>
{
    private FirebaseAuth auth;

    public FirebaseUser User => auth.CurrentUser;
    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public async Task<AuthResult> CreateUserAsync(string email, string password)
    {
        try
        {
            return await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        }
        catch (FirebaseException e)
        { 
            throw e; 
        }       
    }

    public async Task<AuthResult> SignInUserAsync(string email, string password)
    {
        try
        {
            return await auth.SignInWithEmailAndPasswordAsync(email, password);
        }
        catch (FirebaseException e)
        {
            throw e;
        }
    }

    public async Task ChangeUserProfile(UserProfile userProfile)
    {
        try
        {
            await User.UpdateUserProfileAsync(userProfile);
        }
        catch (Exception e)
        {

            throw e;
        }
    }

    public void Logout()
    {
        auth.SignOut();
    }

    public string GetErrorMessage(FirebaseException exception)
    {
        switch ((AuthError)exception.ErrorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                return "Email already exists.";
            case AuthError.MissingPassword:
                return "Pleas enter a password.";
            case AuthError.WeakPassword:
                return "Password is to short";
            case AuthError.WrongPassword:
                return "Password is wrong";
            case AuthError.EmailAlreadyInUse:
                return "Email already exists.";
            case AuthError.InvalidEmail:
                return "Invalid email address.";
            case AuthError.MissingEmail:
                return "Please enter email.";
            default:
                return "Fail";
        }
    }
}
