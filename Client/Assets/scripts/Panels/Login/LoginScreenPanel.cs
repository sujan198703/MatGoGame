using UnityEngine;

public class LoginScreenPanel : MonoBehaviour
{
    public bool LoggedIn()
    {
        if (PlayerPrefs.GetInt("SingedInWithFacebook") == 1 || PlayerPrefs.GetInt("SignedInWithGoogle") == 1) return true;
        else return false;

    }

    public void SkipLoginScreen()
    {
        LoginManager.instance.loginScreenPanel.gameObject.SetActive(false);
        LoginManager.instance.loadingScreenPanel.gameObject.SetActive(true);
    }
}
