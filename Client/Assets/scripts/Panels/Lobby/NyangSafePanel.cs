using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NyangSafePanel : MonoBehaviour, PlayerDataStorageInterface
{
    // Public Variables
    [SerializeField] private Text nyangsPocketText;
    [SerializeField] private Text userInputDepositNyangText;
    [SerializeField] private Text userInputWithdrawNyangText;
    [SerializeField] private Text nyangsSafeText;
    [SerializeField] public Text depositSuccessfulBannerText;
    [SerializeField] public Text withdrawSuccessfulBannerText;
    [SerializeField] public GameObject depositSuccessfulBanner;
    [SerializeField] public GameObject withdrawSuccessfulBanner;
    [SerializeField] public Button depositNyangButton;
    [SerializeField] public Button withdrawNyangButton;

    // Private Variables
    private int nyangsPocket;
    private int nyangsSafe;
    private int nyangsTotal;
    private int safeTier;
    private int unitCounter;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void OnEnable() { PlayerDataStorageManager.instance.LoadGame(); UpdateStats(); }

    void UpdateStats()
    {
        nyangsPocketText.text = nyangsPocket.ToString();
        userInputDepositNyangText.text = "0";
        userInputWithdrawNyangText.text = "0";
        nyangsPocketText.text = nyangsPocket.ToString();
        nyangsSafeText.text = nyangsSafe.ToString();
    }

    public void Button_Deposit(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputDepositNyangText.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputDepositNyangText.text = nyangsPocket.ToString();
        }
        else if (buttonName.Equals("Delete"))
        {
            if (userInputDepositNyangText.text.Length > 0)
                userInputDepositNyangText.text = 
                    userInputDepositNyangText.text.Remove(userInputDepositNyangText.text.Length - 1, 1);
            
            if (userInputDepositNyangText.text.Length == 0)
                userInputDepositNyangText.text = "0";
        }
        else if (buttonName.Equals("Divide"))
        {
            if (userInputDepositNyangText.text != "0")
            {
                // Tens
                if (unitCounter == 0)
                {
                    if (userInputDepositNyangText.text.Length > 2) userInputDepositNyangText.text = userInputDepositNyangText.text.Substring(0, 2);
                    else if (userInputDepositNyangText.text.Length < 2) userInputDepositNyangText.text += "0";
                }
                // Thousands
                else if (unitCounter == 1)
                {
                    if (userInputDepositNyangText.text.Length > 4) userInputDepositNyangText.text = userInputDepositNyangText.text.Substring(0, 4);
                    else if (userInputDepositNyangText.text.Length < 4) while (userInputDepositNyangText.text.Length < 4) userInputDepositNyangText.text += "0";
                }
                // Millions
                else if (unitCounter == 2)
                {
                    if (userInputDepositNyangText.text.Length > 7) userInputDepositNyangText.text = userInputDepositNyangText.text.Substring(0, 7);
                    else if (userInputDepositNyangText.text.Length < 7) while (userInputDepositNyangText.text.Length < 7) userInputDepositNyangText.text += "0";
                }

                if (unitCounter < 2) unitCounter++;
                else unitCounter = 0;
            }
        }
        else if (buttonName.Equals("Deposit"))
        {
            Deposit();
        }
        // Number buttons (0-9)
        else
        {
            if (userInputDepositNyangText.text.Equals("0")) userInputDepositNyangText.text = "";

            userInputDepositNyangText.text += buttonName;
        }

        // Cap
        if (int.Parse(userInputDepositNyangText.text) > SafeLimitAmount()) 
        {
            userInputDepositNyangText.text = SafeLimitAmount().ToString();
        }

        // Disable
        if (userInputDepositNyangText.text.Equals("0"))
            depositNyangButton.interactable = false;
        else
            depositNyangButton.interactable = true;
    }

    public void Button_Withdraw(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputWithdrawNyangText.text = "0";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputWithdrawNyangText.text = nyangsSafe.ToString();
        }
        else if (buttonName.Equals("Delete"))
        {
            if (userInputWithdrawNyangText.text.Length > 0)
                userInputWithdrawNyangText.text =
                    userInputWithdrawNyangText.text.Remove(userInputWithdrawNyangText.text.Length - 1, 1);

            if (userInputWithdrawNyangText.text.Length == 0)
                userInputWithdrawNyangText.text = "0";
        }
        else if (buttonName.Equals("Divide"))
        {
            if (userInputWithdrawNyangText.text != "0")
            {
                // Tens
                if (unitCounter == 0)
                {
                    if (userInputWithdrawNyangText.text.Length > 2) userInputWithdrawNyangText.text = userInputWithdrawNyangText.text.Substring(0, 2);
                    else if (userInputWithdrawNyangText.text.Length < 2) userInputWithdrawNyangText.text += "0";
                }
                // Thousands
                else if (unitCounter == 1)
                {
                    if (userInputWithdrawNyangText.text.Length > 4) userInputWithdrawNyangText.text = userInputWithdrawNyangText.text.Substring(0, 4);
                    else if (userInputWithdrawNyangText.text.Length < 4) while (userInputWithdrawNyangText.text.Length < 4) userInputWithdrawNyangText.text += "0";
                }
                // Millions
                else if (unitCounter == 2)
                {
                    if (userInputWithdrawNyangText.text.Length > 7) userInputWithdrawNyangText.text = userInputWithdrawNyangText.text.Substring(0, 7);
                    else if (userInputWithdrawNyangText.text.Length < 7) while (userInputWithdrawNyangText.text.Length < 7) userInputWithdrawNyangText.text += "0";
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
            if (userInputWithdrawNyangText.text.Equals("0")) userInputWithdrawNyangText.text = "";

            userInputWithdrawNyangText.text += buttonName;
        }

        // Cap
        if (int.Parse(userInputWithdrawNyangText.text) > SafeLimitAmount())
        {
            userInputWithdrawNyangText.text = SafeLimitAmount().ToString();
        }

        // Disable
        if (userInputWithdrawNyangText.Equals("0"))
            withdrawNyangButton.interactable = false;
        else
            withdrawNyangButton.interactable = true;
    }

    public void Deposit()
    {
        // If has enough money to deposit
        if (int.Parse(userInputDepositNyangText.text) <= nyangsPocket)
        {
            // Show banner
            depositSuccessfulBanner.SetActive(true);

            // Subtract from user pocket
            nyangsPocket -= int.Parse(userInputDepositNyangText.text);

            // Add to safe amount
            nyangsSafe += int.Parse(userInputDepositNyangText.text);

            // Update total
            nyangsTotal = nyangsPocket + nyangsSafe;

            // Update banner text
            depositSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputDepositNyangText.text + "칩</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>예금</color> <color=white>되었습니다</color>";

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
        if (int.Parse(userInputWithdrawNyangText.text) <= nyangsSafe)
        {
            // Show banner
            withdrawSuccessfulBanner.SetActive(true);

            // Subtract from safe amount
            nyangsSafe -= int.Parse(userInputWithdrawNyangText.text);

            // Add to user pocket
            nyangsPocket += int.Parse(userInputWithdrawNyangText.text);

            // Update total
            nyangsTotal = nyangsPocket + nyangsSafe;

            // Update banner text
            withdrawSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawNyangText.text + "칩</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>출금</color> <color=white>되었습니다</color>";

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
