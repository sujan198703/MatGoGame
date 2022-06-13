using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private bool CanLogin;
    private Backend backend;

    [SerializeField] private GameObject LoginPage;
    [SerializeField] private GameObject LoginPopUp;

    public enum LoginState
    {
        Loggedin,
        Loggedout
    }

    public LoginState loginState;

    private void Start()
    {
        backend = Backend.instance;
        loginState = LoginState.Loggedout;
    }

    private void Update()
    {
        switch (loginState)
        {
            case LoginState.Loggedin:
                LoginPage.SetActive(false);
                break;
            case LoginState.Loggedout:
                LoginPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CheckConnection()
    {
        if (backend.isConnectedToServer())
        {
            loginState = LoginState.Loggedin;

        }
        else
        {
            loginState = LoginState.Loggedout;
        }
    }

    public void CloseLogin()
    {
        LoginPopUp.SetActive(false);
    }
}
