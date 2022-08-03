using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [Header("Panels")]
    [SerializeField] GameObject giftPanel;
    [SerializeField] GameObject itemPanel;
    [SerializeField] GameObject mailPanel;

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

    int unreadNotificationsGiftTab;
    int unreadNotificationsItemTab;
    int unreadNotificationsMailTab;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {
        UpdateTexts();
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
        CheckIfContentsEmpty();
        UpdateContents();
>>>>>>> Stashed changes
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

        // NOTIFICATION BUBBLES
        giftTabNotificationBubbleText.text = unreadNotificationsGiftTab.ToString();
        itemTabNotificationBubbleText.text = unreadNotificationsItemTab.ToString();
        mailTabNotificationBubbleText.text = unreadNotificationsMailTab.ToString();
    }

    // Quest Reward, Event Reward, Free Charging Station Reward, Ad Reward, Etc
    void UpdateGiftPanel()
    {

    }

<<<<<<< Updated upstream
    public void ClaimGift()
=======
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
        // Add mail
        this.mailTabContent.Add(mailTabContent);

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
>>>>>>> Stashed changes
    {

    }

    public void ClaimItem()
    {

    }

    public void ReadMessage()
    {
        PopupManager.instance.inventoryMailPopup.gameObject.SetActive(true);
        PopupManager.instance.inventoryMailPopup.UpdateMailPopup("");
    }

    public void LoadData(PlayerDataManager data)
    {
        unreadNotificationsGiftTab = data.unreadNotificationsInventoryPanel_GiftTab;
        unreadNotificationsItemTab = data.unreadNotificationsInventoryPanel_ItemTab;
        unreadNotificationsMailTab = data.unreadNotificationsInventoryPanel_MailTab;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.unreadNotificationsInventoryPanel_GiftTab = unreadNotificationsGiftTab;
        data.unreadNotificationsInventoryPanel_ItemTab = unreadNotificationsItemTab;
        data.unreadNotificationsInventoryPanel_MailTab = unreadNotificationsMailTab;
    }
}

[System.Serializable]
public class InventoryGift
{

}

[System.Serializable]
public class InventoryItem
{

}

[System.Serializable]
public class InventoryMail
{

}