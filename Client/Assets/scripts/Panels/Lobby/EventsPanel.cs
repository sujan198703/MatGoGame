using UnityEngine;

public class EventsPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [HideInInspector] public int unreadNotifications;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);
    
    public void LoadData(PlayerDataManager data)
    {
        unreadNotifications = data.unreadNotificationsEventsPanel;
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }

}
