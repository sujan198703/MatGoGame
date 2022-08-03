using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeRefillPaymentPopup : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] private TMP_Text freeRefillPaymentGuidanceText;

    private int refillsLeft;
    private int nyangsPocket;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void OnEnable() => UpdateValues();

    void UpdateValues()
    {
        PlayerDataStorageManager.instance.LoadGame();
        UpdateText();
    }

    void UpdateText() => freeRefillPaymentGuidanceText.text = "무료 리필 30만냥을 받으시겠습니까? (<color=red>" + refillsLeft + " 회</color>남음)";

    public void Get300000()
    {
        // Add 300,000 nyang for free
        if (refillsLeft > 0)
        {
            nyangsPocket += 300000;
            refillsLeft--;
        }

        // Update player data
        PlayerDataStorageManager.instance.SaveGame();
        PlayerDataStorageManager.instance.LoadGame();

        this.gameObject.SetActive(false);

        PopupManager.instance.freeRefillPaymentCompletedPopup.UpdateText("리필머니 <color=red>300,000</color>이 지급되었습니다.");
        PopupManager.instance.freeRefillPaymentCompletedPopup.gameObject.SetActive(true);
    }

    public void Get500000()
    {
        // Show rewarded video then add 500,000 nyang
        if (refillsLeft > 0) AdManager.instance.ShowRewardedAd(2);

        // Update player data
        PlayerDataStorageManager.instance.SaveGame();
        PlayerDataStorageManager.instance.LoadGame();

        this.gameObject.SetActive(false);

        PopupManager.instance.freeRefillPaymentCompletedPopup.UpdateText("리필머니 <color=red>500,000</color>이 지급되었습니다.");
        PopupManager.instance.freeRefillPaymentCompletedPopup.gameObject.SetActive(true);
    }

    public void Get500000Reward()
    {
        nyangsPocket += 500000;
        refillsLeft--;
    }

    public void LoadData(PlayerDataManager data)
    {
        nyangsPocket = data.nyangsPocket;
        refillsLeft = data.refillsLeft;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = nyangsPocket;
        data.refillsLeft = refillsLeft;
    }
}
