using Facebook.Unity;
using Google;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour, PlayerDataStorageInterface
{
    [Header("GUI")]
    [SerializeField] TextMeshProUGUI currentNyangs;
    [SerializeField] TextMeshProUGUI currentMatgoChips;
    [SerializeField] TextMeshProUGUI currentRubies;

    // Static Variables
    public static LobbyManager instance { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;

        PlayerDataStorageManager.instance.AddToDataStorageObjects(this);
    }

    void Start() { PlayerDataStorageManager.instance.LoadGame(); }
    
    public void LoadData(PlayerDataManager data)
    {
        currentNyangs.text = data.nyangsPocket.ToString();
        currentMatgoChips.text = data.chipsPocket.ToString();
        currentRubies.text = data.rubies.ToString();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = int.Parse(currentNyangs.text);
        data.chipsPocket = int.Parse(currentMatgoChips.text);
        data.rubies = int.Parse(currentRubies.text);
    }
}
