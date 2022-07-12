using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NyangSafePanel : MonoBehaviour, PlayerDataStorageInterface
{
    // Public Variables
    [SerializeField] private Text totalNyang;
    [SerializeField] private Text userInputDepositNyang;
    [SerializeField] private Text userInputWithdrawNyang;
    [SerializeField] private Text safeNyang;
    [SerializeField] public Text depositSuccessfulBannerText;
    [SerializeField] public Text withdrawSuccessfulBannerText;
    [SerializeField] public GameObject depositSuccessfulBanner;
    [SerializeField] public GameObject withdrawSuccessfulBanner;

    // Private Variables
    private int nyangsPocket;
    private int nyangsSafe;
    private int nyangsTotal;
    private int safeTier;
    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    private void Start() => UpdateStats();

    public void Button_Deposit(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputDepositNyang.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputDepositNyang.text = nyangsPocket.ToString();
        }
        else if (buttonName.Equals("Delete"))
        {
            if (userInputDepositNyang.text.Length > 0)
                userInputDepositNyang.text = 
                    userInputDepositNyang.text.Remove(userInputDepositNyang.text.Length - 1, 1);
            
            if (userInputDepositNyang.text.Length == 0)
                userInputDepositNyang.text = "0";
        }
        else if (buttonName.Equals("Divide"))
        {

        }
        else if (buttonName.Equals("Deposit"))
        {
            Deposit();
        }
        // Number buttons (0-9)
        else
        {
            if (userInputDepositNyang.text.Equals("0")) userInputDepositNyang.text = "";

            userInputDepositNyang.text += buttonName;
        }
    }

    public void Button_Withdraw(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputWithdrawNyang.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputWithdrawNyang.text = nyangsPocket.ToString();
        }
        else if (buttonName.Equals("Delete"))
        {
            if (userInputWithdrawNyang.text.Length > 0)
                userInputWithdrawNyang.text =
                    userInputWithdrawNyang.text.Remove(userInputWithdrawNyang.text.Length - 1, 1);

            if (userInputWithdrawNyang.text.Length == 0)
                userInputWithdrawNyang.text = "0";
        }
        else if (buttonName.Equals("Divide"))
        {

        }
        else if (buttonName.Equals("Withdraw"))
        {
            Withdraw();
        }
        // Number buttons (0-9)
        else
        {
            if (userInputWithdrawNyang.text.Equals("0")) userInputWithdrawNyang.text = "";

            userInputWithdrawNyang.text += buttonName;
        }
    }

    public void Deposit()
    {
        // If has enough money to deposit
        if (int.Parse(userInputDepositNyang.text) <= nyangsPocket)
        {
            // Show banner
            depositSuccessfulBanner.SetActive(true);

            // Update banner text
            depositSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputDepositNyang.text + "칩</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>예금</color> <color=white>되었습니다</color>";

            // Hide banner
            StartCoroutine(HideDepositSuccessfulBanner());

            // Update stats
            UpdateStats();
        }
    }

    IEnumerator HideDepositSuccessfulBanner()
    {
        yield return new WaitForSeconds(1.0f);
        depositSuccessfulBanner.SetActive(false);
    }

    public void Withdraw()
    {
        if (int.Parse(userInputWithdrawNyang.text) <= nyangsSafe)
        {
            // Show banner
            withdrawSuccessfulBanner.SetActive(true);

            // Update banner text
            withdrawSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawNyang.text + "칩</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>출금</color> <color=white>되었습니다</color>";

            // Hide banner
            StartCoroutine(HideWithdrawSuccessfulBanner());

            // Update stats
            UpdateStats();
        }
    }

    IEnumerator HideWithdrawSuccessfulBanner()
    {
        yield return new WaitForSeconds(1.0f);
        withdrawSuccessfulBanner.SetActive(false);
    }

    void UpdateStats()
    {
        totalNyang.text = nyangsTotal.ToString();
        userInputDepositNyang.text = "0";
        userInputWithdrawNyang.text = "0";
        safeNyang.text = nyangsSafe.ToString();
    }

    public void SafeLimitExtensionButton()
    {
        // Update safe tier (basic -> silver -> gold)
        if (safeTier < 2) safeTier++;
    }

    public void LoadData(PlayerDataManager data)
    {
        nyangsPocket = data.nyangsPocket;
        nyangsSafe = data.nyangsSafe;
        nyangsTotal = data.nyangsTotal;
        safeTier = data.safeTier;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = nyangsPocket;
        data.nyangsSafe = nyangsSafe;
        data.nyangsTotal = nyangsTotal;
        safeTier = data.safeTier;
    }
}
