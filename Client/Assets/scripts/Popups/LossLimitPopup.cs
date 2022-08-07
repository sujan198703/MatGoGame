using UnityEngine;
using UnityEngine.UI;

public class LossLimitPopup : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Toggle[] lossLimitToggles;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => PlayerDataStorageManager.instance.LoadGame();

    long lossLimit;
    long tempLossLimit;

    public void UpdateLossLimit(int lossLimitIndex)
    {
        // Update loss limit index
        switch (lossLimitIndex)
        {
            case 0:
                tempLossLimit = 6600000000;
                break;
            case 1:
                tempLossLimit = 13200000000;
                break;
            case 2:
                tempLossLimit = 19800000000;
                break;
            case 3:
                tempLossLimit = 26400000000;
                break;
            case 4:
                tempLossLimit = 33000000000;
                break;
        }
    }

    public void UpdateToggle(Toggle thisToggle)
    {
        // Disable other toggles
        foreach (Toggle t in lossLimitToggles)
        {
            if (thisToggle != t) t.isOn = false;
        }
    }

    public void ChangeLossLimit()
    {
        lossLimit = tempLossLimit;
        PlayerDataStorageManager.instance.SaveGame();
        PopupManager.instance.lossLimitChangeCompletedPopup.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void LoadData(PlayerDataManager data)
    {
        lossLimit = data.dailyLossLimit;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.dailyLossLimit = lossLimit;
    }
}
