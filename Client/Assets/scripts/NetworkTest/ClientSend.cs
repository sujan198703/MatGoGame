using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTcpData(DotnetPacket.Packet _toServer)
    {
        _toServer.WriteLength();
        Client.instance.tcp.SendData(_toServer);
    }

    public static void WelcomePacketRecieved()
    {
        using (DotnetPacket.Packet _packet = new DotnetPacket.Packet((int)DotnetPacket.ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(Client.instance.inputField.text);

            SendTcpData(_packet);
        }
    }
}
