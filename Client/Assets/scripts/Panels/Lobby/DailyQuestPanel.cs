using UnityEngine;

public class DailyQuestPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [HideInInspector] public int unreadNotifications;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    public void LoadData(PlayerDataManager data)
    {
        unreadNotifications = data.unreadNotificationsDailyQuestPanel;
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }

    public int UnreadNotifications()
    {
        return 0;
    }
}
