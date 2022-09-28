using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GiftTabContent : MonoBehaviour, PlayerDataStorageInterface
{
    public Text giftTabContentText;
    public Text giftTabContentValueText;
    public Button giftTabContentClaimButton;
    public Image giftTabContentProgressBar;

    private List<GiftTabContent> giftTabContent = new List<GiftTabContent>();
    private List<GiftTabContent> giftTabContentClaimed = new List<GiftTabContent>();

    private string uniqueID = "";
    private int unreadGiftNotifications;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {
        PlayerDataStorageManager.instance.LoadGame();

        // If claimed
        foreach (GiftTabContent gtc in giftTabContentClaimed)
        {
            if (gtc.GetUniqueID().Equals(this.GetUniqueID()))
            {
                Claimed();
            }
        }
    }

    public void ClaimGift()
    {
        Claimed();
    }

    void Claimed()
    {
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        }

        foreach (Text txt in GetComponentsInChildren<Text>())
        {
            txt.color = Color.white;
        }
        
        // Replace Red Date with Black
        giftTabContentText.text = giftTabContentText.text.Replace("red", "black");

        giftTabContentClaimButton.interactable = false;

        // Decrement counter
        if (unreadGiftNotifications > 0) unreadGiftNotifications--;

        // Reload game
        PlayerDataStorageManager.instance.SaveThenLoad();
    }

    // We don't need this, unless our case is like mail tab content
    //void Unclaimed()
    //{
    //    foreach (Image img in GetComponentsInChildren<Image>())
    //    {
    //        img.material = null;
    //    }

    //    foreach (Text txt in GetComponentsInChildren<Text>())
    //    {
    //        txt.color = Color.red;
    //    }

    //    // Replace Black Date with Red
    //    giftTabContentText.text = giftTabContentText.text.Replace("black", "red");

    //    giftTabContentClaimButton.interactable = true;
    //}

    public void SetUniqueID(string _uniqueID) => uniqueID = _uniqueID;

    public string GetUniqueID() { return (giftTabContentText.text + giftTabContentValueText.text).GetHashCode().ToString(); }

    public void LoadData(PlayerDataManager data)
    {
        giftTabContent = data.giftTabContent;
        giftTabContentClaimed = data.giftTabContentClaimed;
        unreadGiftNotifications = data.unreadNotificationsInventoryPanel_MailTab;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.giftTabContent = giftTabContent;
        data.giftTabContentClaimed = giftTabContentClaimed;
        data.unreadNotificationsInventoryPanel_MailTab = unreadGiftNotifications;
    }
}
