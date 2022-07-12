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

    public void ClaimGift()
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