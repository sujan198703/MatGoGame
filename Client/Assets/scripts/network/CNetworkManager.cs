using UnityEngine;
using FreeNet;
using WebSocketSharp;
using Newtonsoft.Json;

public interface IMessageReceiver
{
    void on_recv(CPacket msg);
}

public class CNetworkManager : CSingletonMonobehaviour<CNetworkManager>
{

    private WebSocket ws;
    public string serverURL = "ws://localhost:8080";
    CLocalServer gameserver;
    string received_msg;
    bool bAvailableConnection = false;

    public IMessageReceiver message_receiver;


    void Awake()
    {
        this.received_msg = "";

        this.gameserver = new CLocalServer();
        this.gameserver.appcallback_on_message += on_message;
        ConnectionStatus();
    }


    public void start_localserver()
    {
        this.gameserver.start_localserver();
    }


    void on_message(CPacket msg)
    {

        this.message_receiver.on_recv(msg);
        CPacket.destroy(msg);
    }

    private void ConnectionStatus()
    {
        //print("Trying Connection......");

        ws = new WebSocket(serverURL);

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected");
            bAvailableConnection = true;
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Not Connected");
            bAvailableConnection = false;
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);
        };

        ws.Connect();
    }

    public void send(CPacket msg)
    {
        var str = JsonConvert.SerializeObject(msg);

        if( bAvailableConnection )
            ws.Send(str);

        NetworkQueue.instance.AddToQueue(msg.ToString());  
        this.gameserver.on_receive_from_client(msg);
        CPacket.destroy(msg);
    }
}
