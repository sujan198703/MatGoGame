using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [Header("GUI")]
    [SerializeField] Toggle[] notifcationToggles;
    [SerializeField] private Text gameVersionText;
    
    bool vibrationEnabled;
    bool notificationsEnabled;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void OnEnable() => PlayerDataStorageManager.instance.LoadGame();

    void UpdateValues()
    {
        // TOGGLES
        if (notificationsEnabled) notifcationToggles[0].isOn = true;
        else if (!notificationsEnabled) notifcationToggles[1].isOn = true;

        gameVersionText.text = Application.version;
    }

    public void ToggleBGM()
    {
        if (SoundManager.instance.bgmSource.mute == true) SoundManager.instance.bgmSource.mute = false;
        else SoundManager.instance.bgmSource.mute = true;
    }

    public void ToggleSFX()
    {
        if (SoundManager.instance.sfxSource.mute == true) SoundManager.instance.sfxSource.mute = false;
        else SoundManager.instance.sfxSource.mute = true;
    }

    public void ToggleVoice()
    {
        if (SoundManager.instance.voiceSource.mute == true) SoundManager.instance.voiceSource.mute = false;
        else SoundManager.instance.voiceSource.mute = true;
    }

    public void ToggleVibration()
    {
        PlayerDataStorageManager.instance.LoadGame();
        if (vibrationEnabled) vibrationEnabled = false;
        else vibrationEnabled = true;
        PlayerDataStorageManager.instance.SaveGame();
    }

    public void ToggleNotification(bool On)
    {
        if (On) notificationsEnabled = true;
        else notificationsEnabled = false;
    }

    public void HowToPlay() => Application.OpenURL("https://www.cardhoarder.com/mtgo-beginner-guide");

    public void CustomerSupport() => Application.OpenURL("https://www.helpscout.com/helpu/definition-of-customer-support/");

    public void TermsAndPolicies() => Application.OpenURL("https://termify.io/privacy-policy-generator?gclid=CjwKCAjwt7SWBhAnEiwAx8ZLao4ze5PoXVbA88vvZuA8rt_Ej5BtY3YnlmdSak3XVnBipmr4vsfZehoCVZ0QAvD_BwE");

    public void LoadData(PlayerDataManager data)
    {
        vibrationEnabled = data.vibrationEnabled;
        notificationsEnabled = data.notificationsEnabled;

        UpdateValues();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.vibrationEnabled = vibrationEnabled;
        data.notificationsEnabled = notificationsEnabled;
    }
}
