using UnityEngine;

public class EventsPanel : MonoBehaviour, PlayerDataStorageInterface
{
    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);
    
    public void LoadData(PlayerDataManager data)
    {
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }
}
