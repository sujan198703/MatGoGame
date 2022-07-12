using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChipSafePanel : MonoBehaviour, PlayerDataStorageInterface
{
    // Public Variables
    [SerializeField] private Text totalChip;
    [SerializeField] private Text userInputDepositChip;
    [SerializeField] private Text userInputWithdrawChip;
    [SerializeField] private Text safeChip;
    [SerializeField] public Text depositSuccessfulBannerText;
    [SerializeField] public Text withdrawSuccessfulBannerText;
    [SerializeField] public GameObject depositSuccessfulBanner;
    [SerializeField] public GameObject withdrawSuccessfulBanner;

    // Private Variables
    private int chipPocket;
    private int chipSafe;
    private int chipTotal;
    private int safeTier;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateStats();

    public void Button_Deposit(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputDepositChip.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputDepositChip.text = chipPocket.ToString();
        }
        else if (buttonName.Equals("Delete"))
        {
            if (userInputDepositChip.text.Length > 0)
                userInputDepositChip.text =
                    userInputDepositChip.text.Remove(userInputDepositChip.text.Length - 1, 1);

            if (userInputDepositChip.text.Length == 0)
                userInputDepositChip.text = "0";
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
            if (userInputDepositChip.text.Equals("0")) userInputDepositChip.text = "";

            userInputDepositChip.text += buttonName;
        }
    }

    public void Button_Withdraw(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputWithdrawChip.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputWithdrawChip.text = chipPocket.ToString();
        }
        else if (buttonName.Equals("Delete"))
        {
            if (userInputWithdrawChip.text.Length > 0)
                userInputWithdrawChip.text =
                    userInputWithdrawChip.text.Remove(userInputWithdrawChip.text.Length - 1, 1);

            if (userInputWithdrawChip.text.Length == 0)
                userInputWithdrawChip.text = "0";
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
            if (userInputWithdrawChip.text.Equals("0")) userInputWithdrawChip.text = "";

            userInputWithdrawChip.text += buttonName;
        }
    }

    public void Deposit()
    {
        // If has enough money to deposit
        if (int.Parse(userInputWithdrawChip.text) <= chipPocket)
        {
            // Show banner
            depositSuccessfulBanner.SetActive(true);

            // Update banner text
            depositSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawChip.text + "만냥</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>예금</color> <color=white>되었습니다</color>";

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
        if (int.Parse(userInputWithdrawChip.text) <= chipSafe)
        {
            // Show banner
            withdrawSuccessfulBanner.SetActive(true);

            // Update banner text
            withdrawSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawChip.text + "만냥</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>출금</color> <color=white>되었습니다</color>";

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
        totalChip.text = chipTotal.ToString();
        userInputDepositChip.text = "0";
        userInputWithdrawChip.text = "0";
        safeChip.text = chipSafe.ToString();
    }

    public void SafeLimitExtensionButton()
    {

        // Update safe tier (basic -> silver -> gold)
        if (safeTier < 2) safeTier++;
    }

    public void LoadData(PlayerDataManager data)
    {
        chipPocket = data.chipsPocket;
        chipSafe = data.chipsSafe;
        chipTotal = data.chipsTotal;
        safeTier = data.safeTier;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = chipPocket;
        data.nyangsSafe = chipSafe;
        data.nyangsTotal = chipTotal;
        safeTier = data.safeTier;
    }
}
