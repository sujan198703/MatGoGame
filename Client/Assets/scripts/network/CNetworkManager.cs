using UnityEngine;
using FreeNet;
using WebSocketSharp;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IMessageReceiver
{
    void on_recv(CPacket msg);
}
public class WPacket
{
    public int isProtocol;
    public PACKET_CODE cmd;
    public string data;
    public int[] listData;
}

public class CNetworkManager : CSingletonMonobehaviour<CNetworkManager>
{
    CLocalServer gameserver;
    string received_msg;
    Queue<CPacket> packet = new Queue<CPacket>();

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
     

    public void on_message(CPacket msg)
    {

        this.message_receiver.on_recv(msg);
        CPacket.destroy(msg);
    }

    public void send(CPacket msg)
    {

        if(GameController.instance.IsMulti()){
            List<string> dataList = new List<string>();
            msg.record_size();
            CPacket clone = CPacket.create(msg.protocol_id);
            clone.overwrite(msg.buffer, 0);
            int num = clone.pop_int16();
            PROTOCOL protocol = (PROTOCOL)clone.pop_protocol_id();  
            for(int i = 0; i < num - 2 ; i ++){
                new AESCrypto();
                string data = AESCrypto.instance.Encrypt(clone.pop_byte().ToString());
                dataList.Add(data);
            }
            string packetstr = GetPacketString((PACKET_CODE)protocol, JsonConvert.SerializeObject(dataList));
            Backend.instance.Send(packetstr);
        }else if (GameController.instance.IsAI()){
            string str = JsonConvert.SerializeObject(msg);
            NetworkQueue.instance.AddToQueue(msg.ToString());  
            this.gameserver.on_receive_from_client(msg);
        }
        CPacket.destroy(msg);
    }
    public string GetPacketString(PACKET_CODE cmd, string data)
    {
        // Packet packet = new Packet();
        // packet.cmd = cmd;
        // packet.data = data;
        WPacket WPacket = new WPacket();
        WPacket.cmd = cmd;
        WPacket.isProtocol = 1;
        WPacket.data = data;
        WPacket.listData = null;

        return JsonUtility.ToJson(WPacket);
    }
}
