using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName;
    [SerializeField] private float splashScreenTime;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject splashPanel;
    [SerializeField] private GameObject launcherPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject avatarPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject pigBankPanel;
    [SerializeField] private GameObject daillyQuestPanel;

    private AudioSource audioSource;
    [SerializeField] private AudioClip button1;
    [SerializeField] private AudioClip button2;

    private void Awake()
    {
        Invoke("DisableSplashScreen", splashScreenTime);
        audioSource = GetComponent<AudioSource>();
    }

    public void DisableSplashScreen()
    {
        splashPanel.SetActive(false);
        launcherPanel.SetActive(true);
    }

    public void FakeLogin()
    {
        launcherPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void OpenProfilePanel()
    {
        profilePanel.SetActive(!profilePanel.activeInHierarchy);
    }

    public void OpenProfileAvatarPanel()
    {
        avatarPanel.SetActive(!avatarPanel.activeInHierarchy);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void OpenVideoPanel()
    {
        videoPanel.SetActive(!videoPanel.activeInHierarchy);
    }
    public void StartGame()
    {
        loadingPanel.SetActive(true);
        //Invoke("LoadGameplayScene", 2);
    }

    public void OpenDailyQuestPanel()
    {
        daillyQuestPanel.SetActive(!daillyQuestPanel.activeInHierarchy);
    }

    private void LoadGameplayScene()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenPigBankPanel()
    {
        pigBankPanel.SetActive(!pigBankPanel.activeInHierarchy);
    }
    public void OpenURL(string websiteURL)
    {
        Application.OpenURL(websiteURL);
    }

    public void PlayButton1Sound()
    {
        audioSource.PlayOneShot(button1);
    }

    public void PlayButton2Sound()
    {
        audioSource.PlayOneShot(button2);
    }
}
