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
    CLocalServer gameserver;
    string received_msg;
    bool bAvailableConnection = false;

    public IMessageReceiver message_receiver;


    void Awake()
    {
        this.received_msg = "";

        this.gameserver = new CLocalServer();
        this.gameserver.appcallback_on_message += on_message;
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

    public void send(CPacket msg)
    {
        var str = JsonConvert.SerializeObject(msg);

        if( bAvailableConnection )
            Backend.instance.Send(str);

        NetworkQueue.instance.AddToQueue(msg.ToString());  
        this.gameserver.on_receive_from_client(msg);
        CPacket.destroy(msg);
    }
}
