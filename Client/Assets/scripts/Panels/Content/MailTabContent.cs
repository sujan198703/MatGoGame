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

    string mailText = "";

    private List<MailTabContent> mailTabContent;
    private List<MailTabContent> mailTabContentClaimed;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {
        PlayerDataStorageManager.instance.LoadGame();

        // If unopened
        if (mailTabContent.Exists(x => x == this)) Read();

        // If opened
        if (mailTabContentClaimed.Exists(x => x == this)) Unread();
    }

    void Read()
    {
        readButtonImage.sprite = Sprite.Create
              (readButton, new Rect(0, 0, readButton.width, readButton.height),
              new Vector2(readButton.width / 2, readButton.height / 2));

        readButtonText.text = "읽지않음";

        mailIconImage.sprite = Sprite.Create
            (mailIconRead, new Rect(0, 0, mailIconRead.width, mailIconRead.height),
          new Vector2(mailIconRead.width / 2, mailIconRead.height / 2));
    }

    void Unread()
    {
        readButtonImage.sprite = Sprite.Create
               (unreadButton, new Rect(0, 0, unreadButton.width, unreadButton.height),
               new Vector2(unreadButton.width / 2, unreadButton.height / 2));

        readButtonText.text = "받기";

        mailIconImage.sprite = Sprite.Create
            (mailIconUnread, new Rect(0, 0, mailIconUnread.width, mailIconUnread.height),
          new Vector2(mailIconUnread.width / 2, mailIconUnread.height / 2));
    }

    public void ReadMail()
    {
        // Update mail read button image
        Read();

        // Add to opened list, if doesn't exists
        if (!mailTabContentClaimed.Exists(x => x == this)) mailTabContentClaimed.Add(this);

        // Remove from list, if exists
        if (mailTabContent.Exists(x => x == this)) mailTabContent.Remove(this);

        // Enable popup
        PopupManager.instance.inventoryMailPopup.gameObject.SetActive(true);

        // Show mail
        PopupManager.instance.inventoryMailPopup.gameObject.SetActive(true);

        // Update object reference
        PopupManager.instance.inventoryMailPopup.UpdateMailTabContentObject(this);

        // Update mail text
        PopupManager.instance.inventoryMailPopup.UpdateMailPopup(mailText);

        // Update mail content index
        PopupManager.instance.inventoryMailPopup.UpdateMailIndex(transform.GetSiblingIndex());
    }

    public void LoadData(PlayerDataManager data)
    {
        mailTabContent = data.mailTabContent;
        mailTabContentClaimed = data.mailTabContentClaimed;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.mailTabContent = mailTabContent;
        data.mailTabContentClaimed = mailTabContentClaimed;
    }
}
 