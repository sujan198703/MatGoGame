using UnityEngine.UI;
using UnityEngine;

public class LoginServer : MonoBehaviour
{
    public InputField Username;
    public InputField Password;

    private void Start()
    {

    }

    public void Login()
    {
        if( AESCrypto.instance == null )
            new AESCrypto();

        string username = AESCrypto.instance.Encrypt(Username.text);
        string password = AESCrypto.instance.Encrypt(Password.text);

        Backend.OnLogin(username, password);
    }
}