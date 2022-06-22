using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoneySafePanel : MonoBehaviour
{
    [SerializeField] private Text totalNyang;
    [SerializeField] private Text userInputDepositNyang;
    [SerializeField] private Text userInputWithdrawNyang;
    [SerializeField] private Text safeNyang;
    [SerializeField] public Text depositSuccessfulBannerText;
    [SerializeField] public Text withdrawSuccessfulBannerText;
    [SerializeField] public GameObject depositSuccessfulBanner;
    [SerializeField] public GameObject withdrawSuccessfulBanner;

    private void Start() => UpdateStats();

    public void Button_Deposit(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputDepositNyang.text = "0만냥";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputDepositNyang.text = PlayerDataStorageManager.instance.playerDataManager.nyangsPocket.ToString()+ "만냥";
        }
        else if (buttonName.Equals("Delete"))
        {
            // If length is zero, show 0
            if (userInputDepositNyang.text.Length == 0)
            {
                userInputDepositNyang.text = "0만냥";
            }
            // Otherwise take away last digit
            else
            {
                userInputDepositNyang.text =
                    userInputDepositNyang.text.Substring(0, userInputDepositNyang.text.Length - 1) + "만냥";
            }
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
            if (userInputDepositNyang.text.Length > 0)
            {
                userInputDepositNyang.text += buttonName + "만냥";
            }
            else
            {
                userInputDepositNyang.text = "0만냥";
            }
        }
    }

    public void Button_Withdraw(string buttonName)
    {
        if (buttonName.Equals("Reset"))
        {
            userInputWithdrawNyang.text = "0만냥";
        }
        else if (buttonName.Equals("MaxAmount"))
        {
            userInputWithdrawNyang.text = PlayerDataStorageManager.instance.playerDataManager.nyangsSafe.ToString() + "만냥";
        }
        else if (buttonName.Equals("Delete"))
        {
            // If length is zero, show 0
            if (userInputWithdrawNyang.text.Length == 0)
            {
                userInputWithdrawNyang.text = "0만냥";
            }
            // Otherwise take away last digit
            else
            {
                userInputWithdrawNyang.text =
                    userInputWithdrawNyang.text.Substring(0, userInputWithdrawNyang.text.Length - 1) + "만냥";
            }
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
            if (userInputWithdrawNyang.text.Length > 0)
            {
                userInputWithdrawNyang.text += buttonName + "만냥";
            }
            else
            {
                userInputWithdrawNyang.text = "0만냥";
            }
        }
    }

    public void Deposit()
    {
        // If has enough money to deposit
        if (int.Parse(userInputDepositNyang.text) <= PlayerDataStorageManager.instance.playerDataManager.nyangsPocket)
        {
            // Show banner
            depositSuccessfulBanner.SetActive(true);

            // Update banner text
            depositSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputDepositNyang.text + "만냥</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>예금</color> <color=white>되었습니다</color>";
           
            // Hide banner
            StartCoroutine(HideDepositSuccessfulBanner());

            // Update stats
            UpdateStats();
        }
        else
        {

        }
    }

    IEnumerator HideDepositSuccessfulBanner()
    {
        yield return new WaitForSeconds(1.0f);
        depositSuccessfulBanner.SetActive(false);
    }

    public void Withdraw()
    {
        // If has enough money to withdraw
        if (int.Parse(userInputWithdrawNyang.text) <= PlayerDataStorageManager.instance.playerDataManager.nyangsSafe)
        {
            // Show banner
            withdrawSuccessfulBanner.SetActive(true);

            // Update banner text
            withdrawSuccessfulBannerText.text = "<b> <size=24> <color=#FFF77EFF>" + userInputWithdrawNyang.text + "만냥</color> </size> </b> <color=white > 금고에서</color> \n <color=#FFF77EFF>출금</color> <color=white>되었습니다</color>";
           
            // Hide banner
            StartCoroutine(HideWithdrawSuccessfulBanner());

            // Update stats
            UpdateStats();
        }
        // Show unsuccessful banner
        else
        {

        }
    }

    IEnumerator HideWithdrawSuccessfulBanner()
    {
        yield return new WaitForSeconds(1.0f);
        withdrawSuccessfulBanner.SetActive(false);
    }

    void UpdateStats()
    {
        totalNyang.text = PlayerDataStorageManager.instance.playerDataManager.nyangsTotal.ToString();
        userInputDepositNyang.text = "0만냥";
        userInputWithdrawNyang.text = "0만냥";
        safeNyang.text = PlayerDataStorageManager.instance.playerDataManager.nyangsSafe.ToString();
    }

    public void SafeLimitExtensionButton()
    {

    }
}
