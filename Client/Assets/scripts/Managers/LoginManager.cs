using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public GameObject loginScreen;
    public LoginScreenPanel loginScreenPanel;
    public UnderMaintenancePanel underMaintenancePanel;
    public DownloadPatchPanel downloadPatchPanel;

    public static LoginManager instance { get; private set; }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (loginScreenPanel.LoggedIn()) loginScreenPanel.ShowLoginScreenPanel();
        if (downloadPatchPanel.PatchAvailable()) downloadPatchPanel.ShowDownloadPatchPanel();
        if (underMaintenancePanel.IsUnderMaintenance()) underMaintenancePanel.ShowUnderMaintenancePanel(); 
    }

    void ShowLoginScreenPanel()
    {
        loginScreenPanel.gameObject.SetActive(true);
        underMaintenancePanel.gameObject.SetActive(false);
        downloadPatchPanel.gameObject.SetActive(false);
    }

    void ShowDownloadPatchPanel()
    {
        downloadPatchPanel.gameObject.SetActive(true);
        loginScreenPanel.gameObject.SetActive(false);
        underMaintenancePanel.gameObject.SetActive(false);
    }

    void ShowUnderMaintenancePanel()
    {
        underMaintenancePanel.gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
        downloadPatchPanel.gameObject.SetActive(false);
    }

    public void GoToLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
