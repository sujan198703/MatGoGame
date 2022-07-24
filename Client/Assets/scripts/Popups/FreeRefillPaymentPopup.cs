using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeRefillPaymentPopup : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] private TMP_Text freeRefillPaymentGuidanceText;

    private int refillsLeft;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    
    void UpdateText(string text)
    {
        freeRefillPaymentGuidanceText.text = "무료 리필 30만냥을 받으시겠습니까? (<color=red>" + refillsLeft + "회</color>남음)";
    }

    public void LoadData(PlayerDataManager data)
    {
        refillsLeft = data.refillsLeft;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.refillsLeft = refillsLeft;
    }
}
