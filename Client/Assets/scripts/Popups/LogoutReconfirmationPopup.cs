using Facebook.Unity;
using Google;
using UnityEngine;

public class LogoutReconfirmationPopup : MonoBehaviour
{
    public void Logout()
    {
        if (PlayerPrefs.GetInt("SignedInWithGoogle") == 1)
        {
            GoogleSignIn.DefaultInstance.SignOut();
        }
        else if (PlayerPrefs.GetInt("SignedInWithFacebook") == 1)
        {
            FB.LogOut();
        }
        else if (PlayerPrefs.GetInt("SignedInWithNaver") == 1)
        {

        }
        else if (PlayerPrefs.GetInt("SignedInWith2WinGams") == 1)
        {

        }
    }
}
