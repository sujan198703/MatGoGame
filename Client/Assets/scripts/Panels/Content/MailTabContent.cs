using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MailTabContent : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Image mailIconImage;
    [SerializeField] Image readButtonImage;
    [SerializeField] Texture2D mailIconRead;
    [SerializeField] Texture2D mailIconUnread;
    [SerializeField] Texture2D readButton;
    [SerializeField] Texture2D unreadButton;
    [SerializeField] Text readButtonText;
    [SerializeField] Text mailTabContentText;

    private List<MailTabContent> mailTabContent = new List<MailTabContent>();
    private List<MailTabContent> mailTabContentClaimed = new List<MailTabContent>();

    private string mailText = "";
    private string uniqueID;

    private int unreadMailNotifications;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {
        PlayerDataStorageManager.instance.LoadGame();

        // If unopened
        foreach (MailTabContent mtc in mailTabContent)
        {
            // Cycle through mail texts to get match with all combinations
            mailText = mtc.mailText;

            if (mtc.GetUniqueID().Equals(this.GetUniqueID()))
            {
                Unread();
                return;
            }
        }

        // If opened
        foreach (MailTabContent mtc in mailTabContentClaimed)
        {
            mailText = mtc.mailText;

            if (mtc.GetUniqueID().Equals(this.GetUniqueID()))
            {
                Read();
                return;
            }
        }
    }

    void Read()
    {
        // GUI
        readButtonImage.sprite = Sprite.Create
              (readButton, new Rect(0, 0, readButton.width, readButton.height),
              new Vector2(readButton.width / 2, readButton.height / 2));

        readButtonText.text = "읽지않음";

        mailIconImage.sprite = Sprite.Create
            (mailIconRead, new Rect(0, 0, mailIconRead.width, mailIconRead.height),
          new Vector2(mailIconRead.width / 2, mailIconRead.height / 2));

        // Decrement counter
        if (unreadMailNotifications > 0) unreadMailNotifications--;
    }

    void Unread()
    {
        // GUI
        readButtonImage.sprite = Sprite.Create
               (unreadButton, new Rect(0, 0, unreadButton.width, unreadButton.height),
               new Vector2(unreadButton.width / 2, unreadButton.height / 2));

        readButtonText.text = "받기";

        mailIconImage.sprite = Sprite.Create
            (mailIconUnread, new Rect(0, 0, mailIconUnread.width, mailIconUnread.height),
          new Vector2(mailIconUnread.width / 2, mailIconUnread.height / 2));
    }

    // Label of the mail
    public void UpdateMailTabContentText(string text)
    {
        string _year = text.Replace("YEAR", DateTime.Now.Year.ToString());
        string _month = _year.Replace("MONTH", DateTime.Now.Month.ToString());
        string _day = _month.Replace("DAY", DateTime.Now.Day.ToString());

        mailTabContentText.text = _day;
    }

    // Mail text upon opening
    public void UpdateMailText(string text)
    {
        mailText = text;
    }

    public void ReadMail()
    {
        // Update mail read button image
        Read();

        // Remove from unclaimed
        if (mailTabContent.Count > 0) mailTabContent.RemoveAt(mailTabContent.Count - 1);

        // Add to claimed
        if (!mailTabContentClaimed.Contains(this))
        {
            MailTabContent _mtc = Instantiate(this, transform.position, transform.rotation);
            mailTabContentClaimed.Add(_mtc);
        }

        // Enable popup
        PopupManager.instance.inventoryMailPopup.gameObject.SetActive(true);

        // Show mail
        PopupManager.instance.inventoryMailPopup.gameObject.SetActive(true);

        // Update object reference
        PopupManager.instance.inventoryMailPopup.UpdateMailTabContentObject(this);

        // Update mail text
        PopupManager.instance.inventoryMailPopup.UpdateMailPopupContentText(mailText);

        // Update mail content index
        PopupManager.instance.inventoryMailPopup.UpdateMailIndex(transform.GetSiblingIndex());

        // Save then load
        PlayerDataStorageManager.instance.SaveThenLoad();

        // Update mail counter
        PanelManager.instance.inventoryPanel.UpdateNotificationTexts();
    }

    public void SetUniqueID(string _uniqueID) => uniqueID = _uniqueID;

    public string GetUniqueID() { return mailText.GetHashCode().ToString(); }

    public void LoadData(PlayerDataManager data)
    {
        mailTabContent = data.mailTabContent;
        mailTabContentClaimed = data.mailTabContentClaimed;
        unreadMailNotifications = data.unreadNotificationsInventoryPanel_MailTab;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.mailTabContent = mailTabContent;
        data.mailTabContentClaimed = mailTabContentClaimed;
        data.unreadNotificationsInventoryPanel_MailTab = unreadMailNotifications;
    }
}
 