using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    // Private Variables
    [Header("Lobby Screen")]
    [SerializeField] TextMeshProUGUI announcementText;
    [SerializeField] TextMeshProUGUI profileLevelText;
    [SerializeField] TextMeshProUGUI profileName;
    [SerializeField] Image profilePicture;
    [SerializeField] Image profileProgressBar;
    [SerializeField] Button luckyTicketTimerButton;
    [SerializeField] Text luckyTicketTimerText;
    float luckyTicketTimerValue = 1800;

    void Start()
    {
        UpdateAnnouncements();
        ResetLuckyTicketTimer();
    }

    void Update()
    {
        if (luckyTicketTimerValue > 0)
        {
            luckyTicketTimerValue -= Time.deltaTime;

            if (luckyTicketTimerButton.interactable) luckyTicketTimerButton.interactable = false;
        }
        else
        {
            luckyTicketTimerValue = 0;

            // Enable button if disabled
            if (!luckyTicketTimerButton.interactable) luckyTicketTimerButton.interactable = true;
        }

        UpdateLuckyTicketTimer(luckyTicketTimerValue);
    }
    void ResetLuckyTicketTimer() => luckyTicketTimerValue = 1800f;

    void UpdateLuckyTicketTimer(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // 0 index hours two digits, 1 index minutes two digits, 2 index seconds two digits 
        luckyTicketTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    void UpdateAnnouncements()
    {
        announcementText.text = "알림";
    }

}
