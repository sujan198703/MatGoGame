using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookAuthController : MonoBehaviour
{
    private string appID = "722713005598752";
    public GameObject loginPanel;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }
    private void InitCallBack()
    {
        if (!FB.IsInitialized)
        {
            FB.ActivateApp();
        }
    }
    private void OnHideUnity(bool isgameshown)
    {
        if (!isgameshown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SignInWithFacebook()
    {
        var permission = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permission, AuthCallBack);
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            //debug.text = (aToken.UserId);
            loginPanel.SetActive(false);
        }
        else
        {
            //debug.text = ("User Cancelled login");
        }
    }
}
