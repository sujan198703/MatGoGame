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
    [SerializeField] public Button depositChipButton;
    [SerializeField] public Button withdrawChipButton;

    // Private Variables
    private int chipPocket;
    private int chipSafe;
    private int chipTotal;
    private int safeTier;
    private int unitCounter;

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
            // Tens
            if (unitCounter == 0)
            {
                if (userInputDepositChip.text.Length > 2) userInputDepositChip.text = userInputDepositChip.text.Substring(0, 2);
                else if (userInputDepositChip.text.Length < 2) userInputDepositChip.text += "0";
            }
            // Thousands
            else if (unitCounter == 1)
            {
                if (userInputDepositChip.text.Length > 4) userInputDepositChip.text = userInputDepositChip.text.Substring(0, 4);
                else if (userInputDepositChip.text.Length < 4) while (userInputDepositChip.text.Length < 4) userInputDepositChip.text += "0";
            }
            // Millions
            else if (unitCounter == 2)
            {
                if (userInputDepositChip.text.Length > 7) userInputDepositChip.text = userInputDepositChip.text.Substring(0, 7);
                else if (userInputDepositChip.text.Length < 7) while (userInputDepositChip.text.Length < 7) userInputDepositChip.text += "0";
            }

            if (unitCounter < 2) unitCounter++;
            else unitCounter = 0;

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

        // Cap
        if (int.Parse(userInputDepositChip.text) > SafeLimitAmount())
        {
            userInputDepositChip.text = SafeLimitAmount().ToString();
        }

        // Disable
        if (userInputDepositChip.text.Equals("0"))
            depositChipButton.interactable = false;
        else
            depositChipButton.interactable = true;
    }

    public void Button_Withdraw(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputWithdrawChip.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputWithdrawChip.text = chipSafe.ToString();
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
            if (userInputWithdrawChip.text != "0")
            {
                // Tens
                if (unitCounter == 0)
                {
                    if (userInputWithdrawChip.text.Length > 2) userInputWithdrawChip.text = userInputWithdrawChip.text.Substring(0, 2);
                    else if (userInputWithdrawChip.text.Length < 2) userInputWithdrawChip.text += "0";
                }
                // Thousands
                else if (unitCounter == 1)
                {
                    if (userInputWithdrawChip.text.Length > 4) userInputWithdrawChip.text = userInputWithdrawChip.text.Substring(0, 4);
                    else if (userInputWithdrawChip.text.Length < 4) while (userInputWithdrawChip.text.Length < 4) userInputWithdrawChip.text += "0";
                }
                // Millions
                else if (unitCounter == 2)
                {
                    if (userInputWithdrawChip.text.Length > 7) userInputWithdrawChip.text = userInputWithdrawChip.text.Substring(0, 7);
                    else if (userInputWithdrawChip.text.Length < 7) while (userInputWithdrawChip.text.Length < 7) userInputWithdrawChip.text += "0";
                }

                if (unitCounter < 2) unitCounter++;
                else unitCounter = 0;
            }
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

        // Cap
        if (int.Parse(userInputWithdrawChip.text) > SafeLimitAmount())
        {
            userInputWithdrawChip.text = SafeLimitAmount().ToString();
        }

        // Disable
        if (userInputWithdrawChip.text.Equals("0"))
            withdrawChipButton.interactable = false;
        else
            withdrawChipButton.interactable = true;
    }

    public void Deposit()
    {
        // If has enough money to deposit
        if (int.Parse(userInputWithdrawChip.text) <= chipPocket)
        {
            // Show banner
            depositSuccessfulBanner.SetActive(true);

            // Subtract from user pocket
            chipPocket -= int.Parse(userInputDepositChip.text);

            // Add to safe amount
            chipSafe += int.Parse(userInputDepositChip.text);

            // Update banner text
            depositSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawChip.text + "만냥</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>예금</color> <color=white>되었습니다</color>";

            // Hide banner
            StartCoroutine(HideDepositSuccessfulBanner());

            // Save game
            PlayerDataStorageManager.instance.SaveGame();

            // Update stats
            UpdateStats();

            // Load game
            PlayerDataStorageManager.instance.LoadGame();
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

            // Subtract from safe amount
            chipSafe -= int.Parse(userInputWithdrawChip.text);

            // Add to user pocket
            chipSafe += int.Parse(userInputWithdrawChip.text);

            // Update banner text
            withdrawSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawChip.text + "만냥</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>출금</color> <color=white>되었습니다</color>";

            // Hide banner
            StartCoroutine(HideWithdrawSuccessfulBanner());

            // Save game
            PlayerDataStorageManager.instance.SaveGame();

            // Update stats
            UpdateStats();

            // Load game
            PlayerDataStorageManager.instance.LoadGame();
        }
    }

    IEnumerator HideWithdrawSuccessfulBanner()
    {
        yield return new WaitForSeconds(1.0f);
        withdrawSuccessfulBanner.SetActive(false);
    }

    long SafeLimitAmount()
    {
        switch (safeTier)
        {
            case 0:
                return 4500000000;
            case 1:
                return 9000000000;
            case 2:
                return 15000000000;
        }

        return 0;
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
        safeTier = data.chipSafeTier;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.nyangsPocket = chipPocket;
        data.nyangsSafe = chipSafe;
        data.nyangsTotal = chipTotal;
        safeTier = data.chipSafeTier;
    }
}
