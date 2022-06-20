using UnityEngine;
using FreeNet;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(DotnetPacket.Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomePacketRecieved();
    }

    public static void RecieveCPacket(string CPacketJson)
    {
        CPacket packet = JsonUtility.FromJson<CPacket>(CPacketJson);
        print(CPacketJson);
    }
}
