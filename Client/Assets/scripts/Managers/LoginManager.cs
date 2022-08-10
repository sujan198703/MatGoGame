using UnityEngine;

public class LoginManager : MonoBehaviour, PlayerDataStorageInterface
{
    public GameObject loginScreen;
    public LoginScreenPanel loginScreenPanel;
    public UnderMaintenancePanel underMaintenancePanel;
    public DownloadPatchPanel downloadPatchPanel;
    public LoadingScreenPanel loadingScreenPanel;

    string loginMethod;

    public static LoginManager instance { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (downloadPatchPanel.PatchAvailable()) downloadPatchPanel.ShowDownloadPatchPanel();
        else if (underMaintenancePanel.IsUnderMaintenance()) underMaintenancePanel.ShowUnderMaintenancePanel();
        else if (loginScreenPanel.LoggedIn()) loginScreenPanel.SkipLoginScreen();
    }

    public void GoToLobby()
    {
        PlayerDataStorageManager.instance.SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(2/*UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1*/);
    }

    public void LoadData(PlayerDataManager data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.loginMethod = loginMethod;
    }
}
