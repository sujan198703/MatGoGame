using UnityEngine;
using UnityEngine.UI;

public class LevelUpRewardsToast : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Text nextLevelIsText;
    [SerializeField] Text levelUpRewardIsText;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start()
    {
        PlayerDataStorageManager.instance.LoadGame();    
    }

    public void ToggleToast()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public void LoadData(PlayerDataManager data)
    {
        nextLevelIsText.text = "다음 레벨은 <color=red>레벨" + data.playerLevel + 1 + "</color> 입니다.";
        levelUpRewardIsText.text = "레벨업 보상은 <color=red>" + "XXX" + "</color>입니다.";
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }
}
