using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PACKET_CODE
{
    UNKNOWN = -1,
    OPEN = 100,
    FIND,
    WAIT,
    PLAY,
    CLOSE,
    CARD,
    GOSTOP,
    GOOUT,
    SLOT,
    BOX,
    SPIN,
    CHAT,
    SYNC,
    MIX,
    BREAK,
    RECONNECT,
    ALLDATA,

    OK = 200,
    ERROR,
    FIND_OK,
    FIND_FAIL,
    START_OK,
    CARD_OK,
    GOSTOP_OK,
    TURN_OK,
    CHAT_OK,

    CHECK = 300,

    LOGIN = 500,
    PROFILE,
    UPDATE,
    STARTGAME,
    ENDGAME,
    GUILDRANKING,
    LEADERBOARD
}

[System.Serializable]
public class Packet
{
    public PACKET_CODE cmd;
    public string data;
}

[System.Serializable]
public class PlayerInfo
{
    public int id;
    public int avatar;
    public int guild;
    public string pname;
    public int coins;
}

[System.Serializable]
public class StartInfo
{
    public int first;
    public int second;
    public int[] precards;
    public int[] cards;
}

[System.Serializable]
public class LoginInfo
{
    public string pname;
    public string pass;
}

[System.Serializable]
public class StartGameInfo
{
    public int user1;
    public int user2;
    public int coin1;
    public int coin2;
}

[System.Serializable]
public class EndGameInfo
{
    public int gameid;
    public int winner;
    public int earned;
    public int user1;
    public int coin3;
    public int coin4;
}

[System.Serializable]
public class LeaderboardInfo
{
    public List<PlayerInfo> lb;
}

[System.Serializable]
public class GuildRankingInfo
{
    public List<int> ranking;
}

public class GuildInfo
{
    public int guild;
    public int score;
}
