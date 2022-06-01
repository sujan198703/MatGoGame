using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Backend : MonoBehaviour
{
    static public Backend instance;
    private enum Status
    {
        Connected,
        NotConnected,
    }

    private Status CurrentStatus;

    [SerializeField] GameObject RetryConnectionPopUp;
    [SerializeField] GameObject LoginFormPopUp;

    public string serverURL = "ws://localhost:8080";

    public UnityWebSocket w;
    private WebSocket ws;

    private bool CanLogin;

    public List<Packet> packets = new List<Packet>();
    public PACKET_CODE response;
    public string rdata;

    // public Player mine;
    // public Player other;

    public int gameId;

    public bool waiting = false;
    public bool connected = false;

    public float timer = 0f;
    public float timerMax = 3;

    static public bool waitGoStop = false;
    static public bool waitSync = true;
    static public bool waitStart = true;
    static public bool goOut = false;
    static public bool received = false;
    static public bool closed = false;
    static public bool breaked = false;
    static public int seed = 0;

    static public LeaderboardInfo lb = new LeaderboardInfo();
    static public GuildInfo[] guilds = new GuildInfo[12];

    void Awake()
    {
        if (instance == null) instance = this;

        waitSync = true;
        closed = false;
        breaked = false;

        for (int i = 0; i < guilds.Length; i++)
        {
            if (guilds[i] == null)
            {
                guilds[i] = new GuildInfo();
                guilds[i].guild = i;
            }
        }
    }

    private void Start()
    {
        CanLogin = false;

        ConnectionStatus();
    }

    public void RetryConnection()
    {
        //if (ws.IsAlive)
        //    return;
        //else
        //{
        //    ConnectionStatus();
        //}


        if (w.IsConnectedtoServer())
            return;
        else
        {
            ConnectionStatus();
        }

    }

    private void ConnectionStatus()
    {
        //print("Trying Connection......");

        //ws = new WebSocket(serverURL);

        //ws.OnOpen += (sender, e) =>
        //{
        //    Debug.Log("Connected");
        //    CanLogin = true;
        //    CurrentStatus = Status.Connected;
        //};

        //ws.OnClose += (sender, e) =>
        //{
        //    Debug.Log("Not Connected");
        //    CurrentStatus = Status.NotConnected;
        //    CanLogin = false;
        //};

        //ws.OnMessage += (sender, e) =>
        //{
        //    Debug.Log(e.Data);
        //    AESCrypto.SetKey(e.Data);
        //    print(AESCrypto.GetKey());
        //};

        //ws.Connect();


        print("Trying Connection......");

        w = new UnityWebSocket(serverURL);

        w.OnOpen += W_OnOpen;

        w.OnClose += W_OnClose;

        w.OnMessage += W_OnMessage;

        w.Connect();
    }

    private void W_OnMessage(UnityWebSocket sender, byte[] data)
    {
        Debug.Log(data);
        AESCrypto.SetKey(data.ToString());
        print(AESCrypto.GetKey());
    }

    private void W_OnClose(UnityWebSocket sender, int code, string reason)
    {
        Debug.Log("Not Connected");
        CurrentStatus = Status.NotConnected;
        CanLogin = false;
    }

    private void W_OnOpen(UnityWebSocket accepted)
    {
        Debug.Log("Connected");
        CanLogin = true;
        CurrentStatus = Status.Connected;
    }

    public void SendPlayData(string data)
    {
        // ws.Send(data);
        w.SendAsync(System.Text.Encoding.ASCII.GetBytes(data));
    }

    void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Connected:
                RetryConnectionPopUp.SetActive(false);
                LoginFormPopUp.SetActive(true);
                break;
            case Status.NotConnected:
                LoginFormPopUp.SetActive(false);
                RetryConnectionPopUp.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SendDetails(PlayerDetails playerDetails)
    {
        if (ws == null || CanLogin == false)
        {
            Debug.Log("not connected!");
            return;
        }

       // ws.Send(JsonUtility.ToJson(playerDetails));

        ws.Send(System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(playerDetails)));
    }

    public bool isConnectedToServer()
    {
        bool isConnected = CanLogin;
        return isConnected;
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
        Debug.Log("Connection closed: " + code + " " + reason);
        connected = false;
        w = null;
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
        w.Close();
        w = null;
        connected = false;
    }

    private void Close()
    {
        if (w != null)
        {
            w.Close();
            w = null;
        }
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

        //packets.Add(packet);
    }

    public void Send(string str)
    {
        if (w == null) return;

        Debug.Log("-> " + str);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
        w.SendAsync(data);
    }

    internal static string GetStringSha256Hash(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        using (var sha = new System.Security.Cryptography.SHA256Managed())
        {
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            byte[] output = new byte[hash.Length / 4];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = (byte)(hash[i * 4] ^ hash[i * 4 + 1] ^ hash[i * 4 + 2] ^ hash[i * 4 + 3]);
            }
            return BitConverter.ToString(output).Replace("-", string.Empty);
        }
    }

    static public void OnLogin(string ename, string epass)
    {
        if (instance == null) return;

        instance.Login(ename, epass);
    }

    public void Login(string pname, string pass)
    {
        LoginInfo info = new LoginInfo();

        info.pname = pname;
        info.pass = pass;

        string packetstr = GetPacketString(PACKET_CODE.LOGIN, info);
        w.SendAsync(System.Text.Encoding.ASCII.GetBytes(packetstr));
        //        Send(packetstr);
        //        StartCoroutine(DoRequest(PACKET_CODE.LOGIN, packetstr, true));
    }

    static public void OnProfile()
    {
        if (instance == null) return;

        instance.Profile();
    }

    public void Profile()
    {
        //  if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
        //  if (mine.id == -1) return;

        string packetstr = GetPacketString(PACKET_CODE.PROFILE, "");
        //StartCoroutine(DoRequest(PACKET_CODE.PROFILE, packetstr));
    }

    static public void OnUpdate()
    {
        if (instance == null) return;

        instance.OnUpdate0();
    }

    public void OnUpdate0()
    {
        //   if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
        //
        //   PlayerInfo info = new PlayerInfo();
        //   info.id = mine.id;
        //   info.avatar = mine.avatar;
        //   info.guild = mine.guild;

        //  string packetstr = GetPacketString(PACKET_CODE.UPDATE, info);
        //   StartCoroutine(DoRequest(PACKET_CODE.UPDATE, packetstr));
    }

    static public void OnStartGame()
    {
        if (instance == null) return;
        instance.StartGame();
    }

    public void StartGame()
    {
        //   if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
        //   if (other == null) other = Player.Find(CARD_OWNER.OTHER);
        //
        //   StartGameInfo info = new StartGameInfo();
        //   info.user1 = mine.id;
        //   info.user2 = other.id;
        //   info.coin1 = mine.coins;
        //   info.coin2 = other.coins;
        //
        //   if (other.type == PLAYER_TYPE.COMPUTER) info.user2 = -1;

        //   string packetstr = GetPacketString(PACKET_CODE.STARTGAME, info);
        //    StartCoroutine(DoRequest(PACKET_CODE.STARTGAME, packetstr));
    }

    //  static public void OnEndGame(CARD_OWNER winner, int earned)
    //  {
    //      if (instance == null) return;
    //      instance.OnEndGame0(winner, earned);
    //  }

    // public void OnEndGame0(CARD_OWNER winner, int earned)
    // {
    //     if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
    //     if (other == null) other = Player.Find(CARD_OWNER.OTHER);
    //
    //     EndGameInfo info = new EndGameInfo();
    //     if (winner == CARD_OWNER.MINE)
    //     {
    //         info.winner = mine.id;
    //     }
    //     if (winner == CARD_OWNER.OTHER)
    //     {
    //         info.winner = other.id;
    //         if (other.type == PLAYER_TYPE.COMPUTER && MultiPlay.goOut == false) info.winner = -1;
    //     }
    //     if (earned < 0) earned *= -1;
    //     info.gameid = gameId;
    //     info.earned = earned;
    //     info.user1 = mine.id;
    //     info.coin3 = mine.coins;
    //     info.coin4 = other.coins;
    //
    //     string packetstr = GetPacketString(PACKET_CODE.ENDGAME, info);
    //     StartCoroutine(DoRequest(PACKET_CODE.ENDGAME, packetstr));
    // }

    static public void OnGuildRanking()
    {
        if (instance == null) return;
        instance.OnGuildRanking0();
    }

    public void OnGuildRanking0()
    {
        string packetstr = GetPacketString(PACKET_CODE.GUILDRANKING, "");
        StartCoroutine(DoRequest(PACKET_CODE.GUILDRANKING, packetstr));
    }

    static public void OnLeaderboard()
    {
        if (instance == null) return;
        instance.OnLeaderboard0();
    }

    public void OnLeaderboard0()
    {
        //  if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
        //  if (mine.id == -1) return;
        //  if (mine.pname == "") return;
        //
        //  PlayerInfo info = new PlayerInfo();
        //  info.id = mine.id;
        //  info.guild = mine.guild;

        //    string packetstr = GetPacketString(PACKET_CODE.LEADERBOARD, info);
        //    StartCoroutine(DoRequest(PACKET_CODE.LEADERBOARD, packetstr));
    }

    IEnumerator DoRequest(PACKET_CODE code, string packetstr, bool waitdlg = false)
    {
        // if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
        //
        // while (waiting == true) yield return null;
        // waiting = true;
        //
        // EventPopup popup = null;
        //    if (waitdlg) popup = PopupWaiting.Show("Please wait...", -1f);

        if (w == null)
        {
            connected = false;
            w = new UnityWebSocket(serverURL);
            w.OnClose += WebSocket_OnClose;
            w.OnOpen += Websocket_OnOpen;
            w.OnMessage += WebSocket_OnMessage;
            w.OnError += WebSocket_OnError;
        }

        int count = 0;
        while (count < 3 && connected == false)
        {
            yield return new WaitForSecondsRealtime(1f);
            count++;
        }
        if (connected == false)
        {
            //    PopupMessage.Show("Can't connect to server. Try again later.");
            //    if (popup != null) popup.Close();
            waiting = false;
            yield break;
        }

        received = false;
        Send(packetstr);

        while (received == false && connected == true) yield return null;
        if (connected == false)
        {
            //      PopupMessage.Show("Connection lost. Try again later.");
            //      if (popup != null) popup.Close();
            waiting = false;
            yield break;
        }

        if (code == PACKET_CODE.LOGIN)
        {
            if (response == PACKET_CODE.OK)
            {
                //     mine.id = int.Parse(rdata);
                //     GamePage.Show(PAGE_TYPE.HOME);
                //     OnProfile0();
            }
            else
            {
                //PopupMessage.Show(rdata);
            }
        }

        if (code == PACKET_CODE.PROFILE)
        {
            if (response == PACKET_CODE.OK)
            {
                PlayerInfo info = new PlayerInfo();
                JsonUtility.FromJsonOverwrite(rdata, info);

                //   mine.pname = info.pname;
                //   mine.avatar = info.avatar;
                //   mine.guild = info.guild;
                //   mine.coins = info.coins;

                OnGuildRanking0();
                OnLeaderboard0();
            }
            else
            {
                //     PopupMessage.Show(rdata);
            }
        }

        if (code == PACKET_CODE.STARTGAME)
        {
            if (response == PACKET_CODE.OK)
            {
                gameId = int.Parse(rdata);
            }
            else
            {
                //     PopupMessage.Show(rdata);
            }
        }

        if (code == PACKET_CODE.LEADERBOARD)
        {
            if (response == PACKET_CODE.OK)
            {
                JsonUtility.FromJsonOverwrite(rdata, lb);
            }
            else
            {
                //      PopupMessage.Show(rdata);
            }
        }

        if (code == PACKET_CODE.GUILDRANKING)
        {
            if (response == PACKET_CODE.OK)
            {
                GuildRankingInfo info = new GuildRankingInfo();
                JsonUtility.FromJsonOverwrite(rdata, info);
                for (int i = 0; i < 12; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (guilds[j].guild == i)
                        {
                            guilds[j].score = info.ranking[i];
                            break;
                        }
                    }
                }

                for (int i = 0; i < 12; i++)
                {
                    for (int j = i + 1; j < 12; j++)
                    {
                        if (guilds[i].score < guilds[j].score)
                        {
                            GuildInfo temp = guilds[j];
                            guilds[j] = guilds[i];
                            guilds[i] = temp;
                        }
                    }
                }
            }
            else
            {
                //        PopupMessage.Show(rdata);
            }
        }

        //  if (popup != null) popup.Close();

        waiting = false;
    }
}
