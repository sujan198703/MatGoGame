using UnityEngine;

public class LoginScreenPanel : MonoBehaviour
{
    public bool LoggedIn()
    {
        if (PlayerPrefs.GetInt("LoggedIn") == 1) return true;
        else return false;
    }

    public void ShowLoginScreenPanel()
    {
        // Enable panel
        gameObject.SetActive(true);
    }
}
