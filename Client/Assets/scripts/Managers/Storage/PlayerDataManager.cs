using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerDataManager
{
    public int nyangsTotal;
    public int nyangsPocket;
    public int nyangsSafe;
    public int rubies;
    public int matgoChips;
    public string playerName;
    public string playerEmail;
    public Image playerProfilePicture;
    
    public PlayerDataManager()
    {
        this.nyangsTotal = 0;
        this.nyangsPocket = 0;
        this.nyangsSafe = 0;
        this.rubies = 0;
        this.matgoChips = 0;
        this.playerName = "";
        this.playerEmail = "";
        this.playerProfilePicture = null;
    }
}
