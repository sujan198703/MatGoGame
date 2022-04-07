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
        playerDetails.username = Username.text;
        playerDetails.password = Password.text;

        backend.SendDetails(playerDetails);
    }
}
