using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : MonoBehaviour
{
    Client client;

    private void Start()
    {
        client = Client.instance;
    }

    public void ConnectToServer()
    {
        client.ConnectToServer();
    }
}
