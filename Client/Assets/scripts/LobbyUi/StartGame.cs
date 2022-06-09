using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject Lobby;

    public void StartGameBtn()
    {
        Lobby.SetActive(false);
    }

    public void QuitGame()
    {
        Lobby.SetActive(true);
    }
}
