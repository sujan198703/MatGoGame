using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [Header("Panels")]
    [SerializeField] GameObject giftPanel;
    [SerializeField] GameObject itemPanel;
    [SerializeField] GameObject mailPanel;

    [Header("Instructions Avatars")]
    [SerializeField] GameObject giftAvatar;
    [SerializeField] GameObject itemAvatar;
    [SerializeField] GameObject mailAvatar;

    [Header("Contents")]
    [SerializeField] GameObject giftPanelContent;
    [SerializeField] GameObject itemPanelContent;
    [SerializeField] GameObject mailPanelContent;

    [Header("No Received")]
    [SerializeField] GameObject noGiftReceived;
    [SerializeField] GameObject noItemReceived;
    [SerializeField] GameObject noMessageReceived;

    [Header("Texts")]
    [SerializeField] Text giftTabDisabledText;
    [SerializeField] Text giftTabEnabledText;
    [SerializeField] Text itemTabDisabledText;
    [SerializeField] Text itemTabEnabledText;
    [SerializeField] Text mailTabDisabledText;
    [SerializeField] Text mailTabEnabledText;

    [Header("Notification Bubbles")]
    [SerializeField] GameObject giftTabNotificationBubble;
    [SerializeField] GameObject itemTabNotificationBubble;
    [SerializeField] GameObject mailTabNotificationBubble;
    [SerializeField] Text giftTabNotificationBubbleText;
    [SerializeField] Text itemTabNotificationBubbleText;
    [SerializeField] Text mailTabNotificationBubbleText;

    [Header("Prefabs")]
    [Tooltip("Pass in prefab references here, then you can add/remove contents at runtime")]
    [SerializeField] List<GiftTabContent> giftTabContentPrefab;
    [SerializeField] List<ItemTabContent> itemTabContentPrefab;
    [SerializeField] List<MailTabContent> mailTabContentPrefab;

    [Header("Content Objects")]
    List<GiftTabContent> giftTabContent = new List<GiftTabContent>();
    List<ItemTabContent> itemTabContent = new List<ItemTabContent>();
    List<MailTabContent> mailTabContent = new List<MailTabContent>();
    List<GiftTabContent> giftTabContentClaimed = new List<GiftTabContent>();
    List<ItemTabContent> itemTabContentClaimed = new List<ItemTabContent>();
    List<MailTabContent> mailTabContentClaimed = new List<MailTabContent>();

    [Header("GreyScale Material")]
    public Material greyScaleMaterial;

    Panels currentPanel = Panels.GiftPanel;

    int unreadNotificationsGiftTab;
    int unreadNotificationsItemTab;
    int unreadNotificationsMailTab;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => PlayerDataStorageManager.instance.LoadGame();

    void UpdateValues()
    {
        //AddMailOnce();
        UpdateTexts();
        CheckIfContentsEmpty();
        UpdateContents();
    }

    void AddMailOnce()
    {
        if (PlayerPrefs.GetInt("MailAdded") == 0)
        {
            // this is just to show everything is working
            AddMail(mailTabContentPrefab[0]);

            PlayerPrefs.SetInt("MailAdded", 1);
        }
    }

    // Update texts
    void UpdateTexts()
    {
        // LABELS
        // Gift tab
        if (unreadNotificationsGiftTab > 0)
        {
            giftTabDisabledText.text = "선물함 (" + unreadNotificationsGiftTab + ")";
            giftTabEnabledText.text = "선물함 (" + unreadNotificationsGiftTab + ")";
        }
        else
        {
            giftTabDisabledText.text = "선물함";
            giftTabEnabledText.text = "선물함";
        }

        // Item tab
        if (unreadNotificationsItemTab > 0)
        {
            itemTabDisabledText.text = "아이템함 (" + unreadNotificationsItemTab + ")";
            itemTabEnabledText.text = "아이템함 (" + unreadNotificationsItemTab + ")";
        }
        else
        {
            itemTabDisabledText.text = "아이템함";
            itemTabEnabledText.text = "아이템함";
        }

        // Mail tab
        if (unreadNotificationsMailTab > 0)
        {
            mailTabDisabledText.text = "쪽지함 (" + unreadNotificationsMailTab + ")";
            mailTabEnabledText.text = "쪽지함 (" + unreadNotificationsMailTab + ")";
        }
        else
        {
            mailTabDisabledText.text = "쪽지함";
            mailTabEnabledText.text = "쪽지함";
        }

        // Notification Bubbles
        giftTabNotificationBubbleText.text = unreadNotificationsGiftTab == 0 ? "N" : unreadNotificationsGiftTab.ToString();
        itemTabNotificationBubbleText.text = unreadNotificationsItemTab == 0 ? "N" : unreadNotificationsItemTab.ToString();
        mailTabNotificationBubbleText.text = unreadNotificationsMailTab == 0 ? "N" : unreadNotificationsMailTab.ToString();
    }

    // Quest Reward, Event Reward, Free Charging Station Reward, Ad Reward, Etc
    void UpdateGiftPanel()
    {

    }

    // ADD / REMOVE FUNCTIONS 
    public void AddGift(GiftTabContent giftTabContent)
    {
        // Add gift
        if (!this.giftTabContent.Exists(x => x == this)) this.giftTabContent.Add(giftTabContent);

        // Increment notification counter
        unreadNotificationsGiftTab++;
    }

    public void AddItem(ItemTabContent itemTabContent)
    {
        // Add item
        this.itemTabContent.Add(itemTabContent);

        // Increment notification counter
        unreadNotificationsItemTab++;
    }

    public void AddMail(MailTabContent mailTabContent)
    {
        // Add mail to list
        this.mailTabContent.Add(mailTabContent);

        // Add to hierarchy
        GameObject tempMailObject = Instantiate(mailTabContent.gameObject, mailPanelContent.transform) as GameObject;

        // Increment notification counter
        unreadNotificationsMailTab++;
    }

    public void RemoveGift(GiftTabContent giftTabContent)
    {
        // Remove gift
        if (this.giftTabContent.Exists(x => x == this)) this.giftTabContent.Remove(giftTabContent);

        // Remove object
        Destroy(giftTabContent.gameObject);

        // Update updates after delay
        Invoke("CheckIfContentsEmpty", 0.1f);
    }

    public void RemoveItem(ItemTabContent itemTabContent)
    {
        // Remove item
        if (this.itemTabContent.Exists(x => x == this)) this.itemTabContent.Remove(itemTabContent);

        // Remove object
        Destroy(itemTabContent.gameObject);

        // Update updates after delay
        Invoke("CheckIfContentsEmpty", 0.1f);
    }

    public void RemoveMail(MailTabContent mailTabContent)
    {
        // Remove mail
        if (this.mailTabContent.Exists(x => x == this)) this.mailTabContent.Remove(mailTabContent);

        // Remove object
        Destroy(mailTabContent.gameObject);

        // Update updates after delay
        Invoke("CheckIfContentsEmpty", 0.1f);

        // Save game
        PlayerDataStorageManager.instance.SaveGame();
        
        // Reload
        PlayerDataStorageManager.instance.LoadGame();
    }

    // Updates root parent objects for their content objects in hierarchy
    void UpdateContents()
    {
        foreach (GiftTabContent gtc in giftTabContent)
        {
            gtc.gameObject.transform.SetParent(giftPanelContent.transform);
        }

        foreach (ItemTabContent itc in itemTabContent)
        {
            itc.gameObject.transform.SetParent(itemPanelContent.transform);
        }

        foreach (MailTabContent mtc in mailTabContent)
        {
            mtc.gameObject.transform.SetParent(mailPanelContent.transform);
        }
    }

    // Checks if any of the contents are empty
    void CheckIfContentsEmpty()
    {
        // Check if contents are empty
        if (giftPanelContent.transform.childCount == 0) noGiftReceived.SetActive(true);
        if (itemPanelContent.transform.childCount == 0) noItemReceived.SetActive(true);
        if (mailPanelContent.transform.childCount == 0) noMessageReceived.SetActive(true);
    }

    // BUTTONS
    public void GiftTabButton()
    {
        // Disable old panel contents 
        DisablePreviousPanelContents(currentPanel);

        // Update current panel
        currentPanel = Panels.GiftPanel;

        // Enable current panel contents
        EnableCurrentPanelContents(currentPanel);
    }

    public void ItemTabButton()
    {
        // Disable old panel contents
        DisablePreviousPanelContents(currentPanel);

        // Update current panel
        currentPanel = Panels.ItemPanel;

        // Enable current panel contents
        EnableCurrentPanelContents(currentPanel);
    }

    public void MailTabButton()
    {
        // Disable old panel contents
        DisablePreviousPanelContents(currentPanel);

        // Update current panel
        currentPanel = Panels.MailPanel;

        // Enable current panel contents
        EnableCurrentPanelContents(currentPanel);
    }

    public void ClaimAllButton()
    {
        switch (currentPanel)
        {
            case Panels.GiftPanel:
                foreach (GiftTabContent gtc in giftPanelContent.GetComponentsInChildren<GiftTabContent>())
                {
                    gtc.ClaimGift();
                }
                break;
            case Panels.ItemPanel:
                foreach (ItemTabContent itc in itemPanelContent.GetComponentsInChildren<ItemTabContent>())
                {
                    itc.ClaimItem();
                }
                break;
            case Panels.MailPanel:
                foreach (MailTabContent mtc in mailPanelContent.GetComponentsInChildren<MailTabContent>())
                {
                    mtc.ReadMail();
                }
                break;
        }

        PlayerDataStorageManager.instance.SaveGame();
    }

    void DisablePreviousPanelContents(Panels previousPanel)
    {
        switch (previousPanel)
        {
            case Panels.GiftPanel:
                giftAvatar.SetActive(false);
                giftPanel.SetActive(false);
                break;
            case Panels.ItemPanel:
                itemAvatar.SetActive(false);
                itemPanel.SetActive(false);
                break;
            case Panels.MailPanel:
                mailAvatar.SetActive(false);
                mailPanel.SetActive(false);
                break;
        }
    }

    void EnableCurrentPanelContents(Panels _currentPanel)
    {
        switch (_currentPanel)
        {
            case Panels.GiftPanel:
                giftAvatar.SetActive(true);
                giftPanel.SetActive(true);
                break;
            case Panels.ItemPanel:
                itemAvatar.SetActive(true);
                itemPanel.SetActive(true);
                break;
            case Panels.MailPanel:
                mailAvatar.SetActive(true);
                mailPanel.SetActive(true);
                break;
        }
    }

    public void LoadData(PlayerDataManager data)
    {
        giftTabContent = data.giftTabContent;
        itemTabContent = data.itemTabContent;
        mailTabContent = data.mailTabContent;
        giftTabContentClaimed = data.giftTabContentClaimed;
        itemTabContentClaimed = data.itemTabContentClaimed;
        mailTabContentClaimed = data.mailTabContentClaimed;

        unreadNotificationsGiftTab = data.unreadNotificationsInventoryPanel_GiftTab;
        unreadNotificationsItemTab = data.unreadNotificationsInventoryPanel_ItemTab;
        unreadNotificationsMailTab = data.unreadNotificationsInventoryPanel_MailTab;

        UpdateValues();
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.giftTabContent = giftTabContent;
        data.itemTabContent = itemTabContent;
        data.mailTabContent = mailTabContent;
        data.giftTabContentClaimed = giftTabContentClaimed;
        data.itemTabContentClaimed = itemTabContentClaimed;
        data.mailTabContentClaimed = mailTabContentClaimed;

        data.unreadNotificationsInventoryPanel_GiftTab = unreadNotificationsGiftTab;
        data.unreadNotificationsInventoryPanel_ItemTab = unreadNotificationsItemTab;
        data.unreadNotificationsInventoryPanel_MailTab = unreadNotificationsMailTab;
    }

    enum Panels { GiftPanel, ItemPanel, MailPanel }
}