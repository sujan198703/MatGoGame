using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using FreeNet;
using Newtonsoft.Json;
public class Backend : CSingletonMonobehaviour<CPlayRoomUI>
{
    static public Backend instance;
    public delegate void MessageHandler(CPacket msg);
    private Queue<Packet> packet_queue = new Queue<Packet>();
    private Queue<WPacket> wpacket_queue = new Queue<WPacket>();
    private enum Status
    {
        Connected,
        NotConnected,
        LoginedIn,
        NotLoginedIn,
        Finding,
        Matched,
    }

    private Status CurrentStatus;

    [SerializeField] GameObject RetryConnectionPopUp;
    [SerializeField] GameObject LoginFormPopUp;

    public string serverURL;

    public UnityWebSocket w;
    private WebSocket ws;

    private bool CanLogin;

    public List<Packet> packets = new List<Packet>();
    public PACKET_CODE response;
    public string rdata;

    public PlayerInfo mine = new PlayerInfo();
    public PlayerInfo other = new PlayerInfo();

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
        Debug.Log("sdfsdfsdd==================" + byte.MaxValue);
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
        if(this.serverURL != "")
            ConnectionStatus();
        CanLogin = false;
        Debug.Log("start");
    }
    void OnDisable()
	{
        if(w != null)
            w.Close();
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
                GameController.instance.ShowLoginPage();
                LoginFormPopUp.SetActive(false);
                RetryConnectionPopUp.SetActive(true);
                break;
            default:
                LoginFormPopUp.transform.parent.gameObject.SetActive(false);
                
                break;
        }

        if(packet_queue.Count > 0)
            PacketQueue(packet_queue.Dequeue());
        if(wpacket_queue.Count > 0)
            RecivedProtocol(wpacket_queue.Dequeue());
    }

    void PacketQueue(Packet packet){
        if(packet.cmd == PACKET_CODE.FIND_MATCHED){
            CurrentStatus = Status.Matched;
            PlayerInfo info = new PlayerInfo();
            JsonUtility.FromJsonOverwrite(packet.data, info);
            other = info;
            GameController.instance.StartMultiGame();
        }
        if(packet.cmd == PACKET_CODE.GOOUT){
            GameController.instance.GoOut();
        }
    }

    public void RetryConnection()
    {

        if (w.IsConnectedtoServer())
            return;
        else
        {
            ConnectionStatus();
        }
    }

    private void ConnectionStatus()
    {
        print("Trying Connection......");
        w = new UnityWebSocket(serverURL);
        w.OnOpen += W_OnOpen;
        w.OnClose += W_OnClose;
        w.OnMessage += W_OnMessage;
        w.Connect();
    }
    #region MessageHandler
    private void W_OnMessage(UnityWebSocket sender, byte[] data)
    {
        AESCrypto.SetKey(data.ToString());
        ServerQueue.instance.AddToQueue(System.Text.Encoding.ASCII.GetString(data));
        if(connected)
            WebSocket_OnMessage(sender, data);
        if(CanLogin)
            connected = true;
    }

    private void WebSocket_OnMessage(UnityWebSocket sender, byte[] data)
    {
        string str = System.Text.Encoding.UTF8.GetString(data);
        if( ParsePacket(str) ){
            received = true;
            Debug.Log("codeReceive<-" + str);
        }
        

    }

    public bool ParsePacket(string data)
    {
        response = PACKET_CODE.UNKNOWN;
        if (data == null) return false;

        Packet packet = new Packet();
        WPacket protocol_data = new WPacket();
        JsonUtility.FromJsonOverwrite(data, protocol_data);
        JsonConvert.DeserializeObject(data);
        JsonUtility.FromJsonOverwrite(data, packet);
        if(protocol_data.isProtocol != null && protocol_data.isProtocol == 0){
            response = packet.cmd;
            rdata = packet.data;
            if(!InspecterResponse(packet)){
                response = PACKET_CODE.UNKNOWN;
                rdata = null;
            }
            return true;
        }else{
            wpacket_queue.Enqueue(protocol_data);
            //RecivedProtocol((PROTOCOL)packet.cmd, protocol_data);
            response = PACKET_CODE.UNKNOWN;
        }
        return false;
        //packets.Add(packet);
    }

    public bool InspecterResponse(Packet packet){
        if(packet.cmd == PACKET_CODE.FIND_MATCHED){
            packet_queue.Enqueue(packet);
            Debug.Log("i matched other opponent!");
            return false;
        }
        if(packet.cmd == PACKET_CODE.GOOUT){
            packet_queue.Enqueue(packet);
            Debug.Log("go out!");
            return false;
        }
        return true;
    }
    
    public void RecivedProtocol(WPacket packet){
        
        CPacket msg = CPacket.create((short)packet.cmd);
        Debug.Log("ReceiveProtocol-->" + packet.cmd + "--data--");
		//msg.push(current_turn_player_index);
        if(packet.listData == null){
            CNetworkManager.Instance.on_message(pre_send(msg));
            return;
        }
        //AESCrypto crypto = new AESCrypto();
        for(int i = 0; i < packet.listData.Length; i++){
            //int data = int.Parse(crypto.Decrypt(packet.listData[i]));
            int data = packet.listData[i];
            if((PROTOCOL)packet.cmd == PROTOCOL.UPDATE_PLAYER_STATISTICS){
                if(i == 2)
                    msg.push((short)data);
                else
                    msg.push((byte)data);
            }else if((PROTOCOL)packet.cmd == PROTOCOL.GAME_RESULT){
                if(i == 1)
                    msg.push((byte)data);
                else
                    msg.push((short)data);
            }else if((PROTOCOL)packet.cmd == PROTOCOL.FLIP_DECK_CARD_ACK){
                // if(i == packet.listData.Length - 3)
                //     msg.push((short)packet.listData[i]);
                // else    
                    msg.push((byte)data);
            }
            else{
                msg.push((byte)data);
            }
        }
        CNetworkManager.Instance.on_message(pre_send(msg));
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

    public CPacket pre_send(CPacket msg)
    {
        msg.record_size();
        CPacket clone = CPacket.create(msg.protocol_id);
        clone.overwrite(msg.buffer, 0);
        clone.pop_int16();
        return clone;
    }

    public void Send(string str)
    {
        if (!w.IsConnectedtoServer()) return;

        Debug.Log("Send-> " + str);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
        w.SendAsync(data);
    }
    #endregion


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

    private void W_OnError(UnityWebSocket sender, string message)
    {
        Debug.Log("Connected");
    }

    public bool isConnectedToServer()
    {
        bool isConnected = CanLogin;
        return isConnected;
    }

    public bool ConnectedToServer()
    {
        return w.IsConnectedtoServer();
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
        //w.SendAsync(System.Text.Encoding.ASCII.GetBytes(packetstr));
        //Send(packetstr);
        StartCoroutine(DoRequest(PACKET_CODE.LOGIN, packetstr, true));
        


    }

    static public void OnProfile(int id)
    {
        if (instance == null) return;

        instance.Profile(id);
    }

    public void Profile(int id)
    {
        //  if (mine == null) mine = Player.Find(CARD_OWNER.MINE);
        //  if (mine.id == -1) return;

        string packetstr = GetPacketString(PACKET_CODE.PROFILE, id.ToString());
        StartCoroutine(DoRequest(PACKET_CODE.PROFILE, packetstr));
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

    private void Close()
    {
        if (w != null)
        {
            w.Close();
            w = null;
        }
        connected = false;
    }

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

    static public void OnOpen(){
        if(instance == null) return;
        instance.Open();
    }

    public void Open(){
        
    
        string packetstr = GetPacketString(PACKET_CODE.OPEN, mine);
        //w.SendAsync(System.Text.Encoding.ASCII.GetBytes(packetstr));
        //Send(packetstr);
        StartCoroutine(DoRequest(PACKET_CODE.OPEN, packetstr, true));
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
            w.OnClose += W_OnClose;
            w.OnOpen += W_OnOpen;
            w.OnMessage += W_OnMessage;
            w.OnError += W_OnError;
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
                mine.id = int.Parse(rdata);
                //     GamePage.Show(PAGE_TYPE.HOME);
                //     OnProfile0();
                OnProfile(mine.id);
                CurrentStatus = Status.LoginedIn;
                GameController.instance.ShowStagePage();
            }
            else
            {
                CUIManager.Instance.show(UI_PAGE.POPUP_MESSAGE, rdata);
            }
        }

        if (code == PACKET_CODE.PROFILE)
        {
            if (response == PACKET_CODE.OK)
            {
                PlayerInfo info = new PlayerInfo();
                JsonUtility.FromJsonOverwrite(rdata, info);
                mine.pname = info.pname;
                mine.avatar = info.avatar;
                mine.guild = info.guild;
                mine.coins = info.coins;
                //OnGuildRanking0();
                //OnLeaderboard0();
            }
            else
            {
                //     PopupMessage.Show(rdata);
            }
        }
        if (code == PACKET_CODE.OPEN){
            if (response != PACKET_CODE.OK && response != PACKET_CODE.ALLDATA)
            {
                Debug.Log("Can't register data.");
            //   ep.Close();
                w.Close();
                yield break;
            }

            if (response == PACKET_CODE.OK)
            {
                Debug.Log("OPEN_OK");
                count = 0;
                while (count < 3)
                {
                    if(CurrentStatus == Status.Matched)
                        yield break;
                    Send(GetPacketString(PACKET_CODE.FIND, null));
                    received = false; while (received == false && connected == true) yield return null;
                    if (connected == false)
                    {
                    //  PopupMessage.Show("Connection lost. Try again later.");
                    //    ep.Close();
                        yield break;
                    }
                    PlayerInfo info = new PlayerInfo();
                    JsonUtility.FromJsonOverwrite(rdata, info);
                    if (response == PACKET_CODE.FIND_OK)
                    {
                        //other = info;
                        //GameController.instance.StartMultiGame();
                        // other.id = info.id;
                        // other.avatar = info.avatar;
                        // other.guild = info.guild;
                        // other.coins = info.coins;
                        // other.pname = info.pname;

                    //  ep.ShowInfo();
                        break;
                    }
                    if(response == PACKET_CODE.FIND_FAIL){
                        Debug.Log("FIND_FAILD");
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

                if (count == 3)
                {
                //   PopupMessage.Show("Can't find opponent. Try again later.");
                //   ep.Close();
                //   w.Close();
                    yield break;
                }

                /*breaked = false;
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
                }*/
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
