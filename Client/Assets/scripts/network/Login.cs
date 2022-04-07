using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private bool CanLogin;
    private Backend backend;
    [SerializeField] private GameObject LoginPage;

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
        print(backend.isConnectedToServer());
        if (backend.isConnectedToServer())
        {
            loginState = LoginState.Loggedin;
        }
        else
        {
            loginState = LoginState.Loggedout;
        }
    }
}
