using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRefillPaymentPopup : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
    {
        
=======
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
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
=======
        data.nyangsPocket = nyangsPocket;
        data.refillsLeft = refillsLeft;
>>>>>>> Stashed changes
    }
}
