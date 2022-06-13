using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultiPlay : MonoBehaviour
{
    static public MultiPlay instance;
    static public StartInfo startInfo = new StartInfo();

    public string serverURL;

    public UnityWebSocket w;
    public PACKET_CODE response;
    public string rdata;

    public List<Packet> packets = new List<Packet>();

    static public bool waitGoStop = false;
    static public bool waitSync = true;
    static public bool waitStart = true;
    static public bool waiting = false;
    static public bool connected = false;
    static public bool goOut = false;
    static public bool received = false;
    static public bool closed = false;
    static public bool breaked = false;
    static public int seed = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Init()
    {
       
    }

    public string GetPacketString(PACKET_CODE cmd, object data)
    {
        return GetPacketString(cmd, JsonUtility.ToJson(data));
    }

    public string GetPacketString(PACKET_CODE cmd, string data)
    {
        Packet packet = new Packet();
        packet.cmd = cmd;
        packet.data = data;

        return JsonUtility.ToJson(packet);
    }

    private void WebSocket_OnClose(UnityWebSocket sender, int code, string reason)
    {
        waitSync = false;
        Debug.Log("Connection closed: " + code + " " + reason);
        connected = false;
        w = null;

        if (closed == false)
        {
            Packet packet = new Packet();
            packet.cmd = PACKET_CODE.BREAK;
            packets.Add(packet);
        }
    }

    private void Websocket_OnOpen(UnityWebSocket accepted)
    {
        connected = true;
    }

    private void WebSocket_OnMessage(UnityWebSocket sender, byte[] data)
    {
        string str = System.Text.Encoding.UTF8.GetString(data);
        ParsePacket(str);
        received = true;

        Debug.Log("<= " + str);
    }

    private void WebSocket_OnError(UnityWebSocket sender, string message)
    {
        Debug.Log("Error: " + message);
        connected = false;
    }

    public void ParsePacket(string data)
    {
        response = PACKET_CODE.UNKNOWN;
        if (data == null) return;

        Packet packet = new Packet();
        JsonUtility.FromJsonOverwrite(data, packet);
        response = packet.cmd;
        rdata = packet.data;

        if (packet.cmd == PACKET_CODE.OK ||
            packet.cmd == PACKET_CODE.FIND_OK ||
            packet.cmd == PACKET_CODE.FIND_FAIL)
        {
            return;
        }

        if (packet.cmd == PACKET_CODE.SYNC)
        {
            waitSync = false;
            return;
        }
        if (packet.cmd == PACKET_CODE.CHECK)
        {
            Send(GetPacketString(PACKET_CODE.CHECK, null), false);
            return;
        }
        if (packet.cmd == PACKET_CODE.START_OK)
        {
            waitStart = false;
            JsonUtility.FromJsonOverwrite(rdata, startInfo);
            return;
        }

        packets.Add(packet);
    }

    IEnumerator DoReconnect()
    {
        string msg = "";
        if (connected == true) msg = "Opponent suddenly disconnected from server. Waiting for reconnection...";
        if (connected == false) msg = "Sorry, server connection is suddenly broken, trying to reconnect...";

       // EventPopup popup = PopupWaiting.Show(msg, -1f);

        if (connected == false)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSecondsRealtime(1.0f);
                w = new UnityWebSocket(serverURL);
                w.OnClose += WebSocket_OnClose;
                w.OnOpen += Websocket_OnOpen;
                w.OnMessage += WebSocket_OnMessage;
                w.OnError += WebSocket_OnError;

                int count = 0;
                while (count < 3 && connected == false)
                {
                    yield return new WaitForSecondsRealtime(1f);
                    count++;
                }
                if (connected == true)
                {
                    PlayerInfo info = new PlayerInfo();
                   /* info.id = mine.id;
                    info.avatar = mine.avatar;
                    info.guild = mine.guild;
                    info.pname = mine.pname;
                    info.coins = mine.coins; */

                    Send(GetPacketString(PACKET_CODE.OPEN, info));
                    received = false; while (received == false) yield return null;
                    if (response != PACKET_CODE.OK)
                    {
                        Debug.Log("Can't register data when reconnect.");
                        break;
                    }

                    Packet packet = new Packet();
                    packet.cmd = PACKET_CODE.ALLDATA;
                    packets.Add(packet);
                   // GameLogic.paused = false;
                    break;
                }
            }
        }
        else
        {
            int count = 0;
            while (count < 30 && breaked == true)
            {
                yield return new WaitForSecondsRealtime(1f);
                count++;
            }
        }

     //   if (popup != null) popup.Close();

        if (breaked == true)
        {
            if (connected == true)
            {
               // PopupMessage.Show("Opponent didn't rejoin the game and you will play with AI.");
                Packet packet = new Packet();
                packet.cmd = PACKET_CODE.GOOUT;
                packets.Add(packet);
              //  GameLogic.paused = false;
            }
            else
            {
               // PopupMessage.Show("Can't reconnect to server. You will lose your current game.");
              //  GameLogic.instance.StopGame();
            }
        }
    }

    IEnumerator DoPlay()
    {
      //  ep = (PopupFindOpponent)EventPopup.GetEventPopup(GAME_EVENT.FIND_OPPONENT);
      //  if (ep != null) ep.ShowEvent();

        w = new UnityWebSocket(serverURL);
        w.OnClose += WebSocket_OnClose;
        w.OnOpen += Websocket_OnOpen;
        w.OnMessage += WebSocket_OnMessage;
        w.OnError += WebSocket_OnError;

        connected = false;
        int count = 0;
        while (count < 3 && connected == false)
        {
            yield return new WaitForSecondsRealtime(1f);
            count++;
        }
        if (connected == false)
        {
         //   PopupMessage.Show("Can't connect to server. Try again later.");
         ///   ep.Close();
            w.Close();
            yield break;
        }

        PlayerInfo info = new PlayerInfo();
      //  info.id = mine.id;
      //  info.avatar = mine.avatar;
      //  info.guild = mine.guild;
      //  info.pname = mine.pname;
      //  info.coins = mine.coins;

        Send(GetPacketString(PACKET_CODE.OPEN, info));
        received = false; while (received == false) yield return null;
        if (response != PACKET_CODE.OK && response != PACKET_CODE.ALLDATA)
        {
            Debug.Log("Can't register data.");
         //   ep.Close();
            w.Close();
            yield break;
        }

        if (response == PACKET_CODE.OK)
        {
            count = 0;
            while (count < 10)
            {
                Send(GetPacketString(PACKET_CODE.FIND, null));
                received = false; while (received == false && connected == true) yield return null;
                if (connected == false)
                {
                  //  PopupMessage.Show("Connection lost. Try again later.");
                //    ep.Close();
                    yield break;
                }
                JsonUtility.FromJsonOverwrite(rdata, info);
                if (response == PACKET_CODE.FIND_OK)
                {
               //    other.id = info.id;
               //    other.avatar = info.avatar;
               //    other.guild = info.guild;
               //    other.coins = info.coins;
               //    other.pname = info.pname;

                  //  ep.ShowInfo();
                    break;
                }
                if (response == PACKET_CODE.GOOUT)
                {
                    goOut = true;
                    received = true;
                    waitSync = false;
                    waitStart = false;
                    waitGoStop = false;
                 //   ep.Close();
                    w.Close();
                    yield break;
                }
                count++;
                yield return new WaitForSecondsRealtime(1f);
            }

            if (count == 10)
            {
            //   PopupMessage.Show("Can't find opponent. Try again later.");
            //   ep.Close();
            //   w.Close();
                yield break;
            }

            breaked = false;
            closed = false;

            yield return new WaitForSecondsRealtime(1f);
            yield return StartCoroutine(DoStart());

          //  ep.Close();
            if (response == PACKET_CODE.START_OK)
            {
            //   GamePage.Show(PAGE_TYPE.ROOM);
            //   other.type = PLAYER_TYPE.NETWORK;
            //
            //   if (startInfo.first == mine.id) firstOwner = CARD_OWNER.MINE;
            //   else firstOwner = CARD_OWNER.OTHER;
            //
            //   GameLogic.instance.gameMode = GAME_MODE.MULTI;
            //   GameLogic.instance.StartGame();
            }
            else
            {
             //   PopupMessage.Show("Sorry, some issue maybe occurred. Can't start game.");
                w.Close();
                yield break;
            }
        }

        if (response == PACKET_CODE.ALLDATA)
        {
           // ep.Close();
        }

        goOut = false;

        while (true)
        {
            while (packets.Count > 0 && (breaked == true))
            {
                Packet packet = packets[0];
                packets.RemoveAt(0);

                //Debug.Log(packet.cmd);

                string data = packet.data;
                if (packet.cmd == PACKET_CODE.CARD)
                {
               //   CardPos card = CardPos.Find(int.Parse(data));
               //   GameLogic.OnCardClick(card, false);
                }
                if (packet.cmd == PACKET_CODE.SLOT)
                {
                    seed = int.Parse(data);
              //  EventPopup ep = EventPopup.GetEventPopup(GAME_EVENT.SLOT_MACHINE);
              //  PopupSlotMachine game = ep.GetComponent<PopupSlotMachine>();
                 //   game.OnClickSpin();
                }
                if (packet.cmd == PACKET_CODE.BOX)
                {
                    string[] datas = data.Split(',');
                    seed = int.Parse(datas[0]);
               //   EventPopup ep = EventPopup.GetEventPopup(GAME_EVENT.MYSTERY_BOX);
               //   PopupMysteryBox game = ep.GetComponent<PopupMysteryBox>();
                   // game.OnBoxClick(int.Parse(datas[1]));
                }
                if (packet.cmd == PACKET_CODE.SPIN)
                {
                    seed = int.Parse(data);
               //   EventPopup ep = EventPopup.GetEventPopup(GAME_EVENT.SPIN_WHEEL);
               //   PopupSpinWheel game = ep.GetComponent<PopupSpinWheel>();
                 //   game.Spin();
                }
                if (packet.cmd == PACKET_CODE.GOSTOP)
                {
                  //  other.go = bool.Parse(data);
                    waitGoStop = false;
                }
                if (packet.cmd == PACKET_CODE.CHAT)
                {
               //     chatView.AddMessageOther(data);
                    Send(GetPacketString(PACKET_CODE.CHAT_OK, null), false);
                }
                if (packet.cmd == PACKET_CODE.CHAT_OK)
                {
                //    chatView.OnSendOK();
                }
                if (packet.cmd == PACKET_CODE.GOOUT)
                {
                    goOut = true;
                    received = true;
                    waitSync = false;
                    waitStart = false;
                    waitGoStop = false;
                    w.Close();

                    if (breaked == false)
                    {
                    //    PopupMessage.Show("Opponent went out the game and you will play with AI.");
                    }

                //   other.type = PLAYER_TYPE.COMPUTER;
                //   other.StartAI();
                //   GameLogic.instance.gameMode = GAME_MODE.SINGLE;
                    yield break;
                }
                if (packet.cmd == PACKET_CODE.BREAK)
                {
                    breaked = true;
                //    GameLogic.paused = true;
                    StartCoroutine(DoReconnect());
                }
                if (packet.cmd == PACKET_CODE.RECONNECT)
                {
                    breaked = false;
                //    GameLogic.paused = false;
                    Send(GetPacketString(PACKET_CODE.ALLDATA, null), false);
                }
                if (packet.cmd == PACKET_CODE.ALLDATA)
                {
                    breaked = false;
                //    GameLogic.paused = false;
                }
            }

            yield return null;
         //  if (GameLogic.instance.gameState == GAME_STATE.OVER)
         //  {
         //      Debug.Log("Multi play ended.");
         //      GameLogic.instance.gameMode = GAME_MODE.UNKNOWN;
         //      if (w != null) w.Close();
         //      yield break;
         //  }
        }
    }

    public IEnumerator DoStart()
    {
        Random.InitState((int)Time.time);

    //  List<Card> precards = Card.GetProcedureCards();
    //  List<Card> cards = Card.GetGameCards();
    //  Card.MixCards(cards);
    //  Card.MixCards(precards);
    //
    //  startInfo.first = mine.id;
    //  startInfo.second = other.id;
    //  startInfo.precards = Card.GetCardIndecies(precards);
    //  startInfo.cards = Card.GetCardIndecies(cards);

        waitStart = true;
        Send(GetPacketString(PACKET_CODE.PLAY, startInfo));
        while (waitStart) yield return true;
    }

    void Send(string str, bool waitResponse = false)
    {
        if (w == null) return;

        Debug.Log("-> " + str);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
        w.SendAsync(data);
    }

    public void OnCardClick0(int index)
    {
        Send(GetPacketString(PACKET_CODE.CARD, "" + index), false);
    }

    public void OnSlot0(int seed)
    {
        Send(GetPacketString(PACKET_CODE.SLOT, "" + seed), false);
    }

    public void OnSpin0(int seed)
    {
        Send(GetPacketString(PACKET_CODE.SPIN, "" + seed), false);
    }

    public void OnBox0(int no, int seed)
    {
        Send(GetPacketString(PACKET_CODE.BOX, seed + "," + no), false);
    }

    public void OnGoStop0(bool go)
    {
        Send(GetPacketString(PACKET_CODE.GOSTOP, "" + go), false);
    }

    public void OnSync0()
    {
        Send(GetPacketString(PACKET_CODE.SYNC, null), false);
    }

    public void OnClose0()
    {
        closed = true;
        Send(GetPacketString(PACKET_CODE.CLOSE, null), false);
    }

    public void OnSendMessage0(string message)
    {
        Send(GetPacketString(PACKET_CODE.CHAT, message), false);
    }

    static public void Start()
    {
        if (instance == null) return;
        instance.Init();
        instance.StartCoroutine(instance.DoPlay());
    }

    static public void StartGame()
    {
        if (instance == null) return;
        instance.StartCoroutine(instance.DoStart());
    }

    static public void OnCardClick(int index)
    {
        if (instance == null) return;
        instance.OnCardClick0(index);
    }

    static public void OnGoStop(bool go)
    {
        if (instance == null) return;
        instance.OnGoStop0(go);
    }

    static public void OnSlot(int seed)
    {
        if (instance == null) return;
        instance.OnSlot0(seed);
    }

    static public void OnSpin(int seed)
    {
        if (instance == null) return;
        instance.OnSpin0(seed);
    }

    static public void OnBox(int no, int seed)
    {
        if (instance == null) return;
        instance.OnBox0(no, seed);
    }

    static public void OnSync()
    {
        if (instance == null) return;
        instance.OnSync0();
    }

    static public void OnSendMessage(string message)
    {
        if (instance == null) return;
        instance.OnSendMessage0(message);
    }

    static public void OnClose()
    {
        if (instance == null) return;
        instance.OnClose0();
    }


}
