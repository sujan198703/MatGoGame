using UnityEngine.UI;
using UnityEngine;

public class LoginServer : MonoBehaviour
{
    public InputField Username;
    public InputField Password;

    private Backend backend;
    private PlayerDetails playerDetails;

    private void Start()
    {
        backend = Backend.instance;
        playerDetails = new PlayerDetails();
    }

    public void SendDetails()
    {
        string username = Username.text;
        string password = Password.text;

        playerDetails.username = Encryption.instance.encrypt(username);
        playerDetails.password = Encryption.instance.encrypt(password);

        backend.SendDetails(playerDetails);
    }
}
