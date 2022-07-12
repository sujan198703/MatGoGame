using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookAuthController : MonoBehaviour
{
    private string appID = "722713005598752";

    // Static Variables
    private static GoogleAuthController _instance;

    public static GoogleAuthController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GoogleAuthController>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

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

            if (PlayerPrefs.GetInt("SingedInWithFacebook") == 1) SignInWithFacebook();
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
#if UNITY_EDITOR
        LoginManager.instance.GoToLobby();
#endif

#if !UNITY_EDITOR
        var permission = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permission, AuthCallBack);
#endif
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var aToken = AccessToken.CurrentAccessToken;
            
            // Get user data
            FB.API("/me?fields=id,name,email", HttpMethod.GET, GetFacebookData, new Dictionary<string, string>() { });

            LoginManager.instance.GoToLobby();
        }
    }

    void GetFacebookData(IGraphResult result)
    {
        PlayerPrefs.SetString("ProfileName", result.ResultDictionary["name"].ToString());
        PlayerPrefs.SetString("ProfileEmail", result.ResultDictionary["email"].ToString());
        PlayerPrefs.SetString("ProfileMembershipCode", result.ResultDictionary["id"].ToString());

        PlayerPrefs.SetInt("SingedInWithFacebook", 1);
    }
}
