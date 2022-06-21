using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerDataManager
{
    public int nyangs;
    public int rubies;
    public int matgoChips;
    public string playerName;
    public string playerEmail;
    public Image playerProfilePicture;
    
    public PlayerDataManager()
    {
        this.nyangs = 0;
        this.rubies = 0;
        this.matgoChips = 0;
        this.playerName = "";
        this.playerEmail = "";
        this.playerProfilePicture = null;
    }
}
