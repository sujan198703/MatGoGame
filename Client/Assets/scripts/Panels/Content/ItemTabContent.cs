using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemTabContent : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Image itemContentIconImage;
    [SerializeField] Image itemClaimButtonImage;
    [SerializeField] Text itemTabContentText;
    [SerializeField] Button itemClaimButton;
    DateTime finishingTime_ItemTabContent = new DateTime();
    DateTime remainingTime_ItemTabContent = new DateTime();
    TimeSpan itemTime = new TimeSpan(3, 0, 0, 0); // Days, Hours, Minutes, Seconds for the object
    TimeSpan remainingTime;

    private List<ItemTabContent> itemTabContent = new List<ItemTabContent>();
    private List<ItemTabContent> itemTabContentClaimed = new List<ItemTabContent>();

    private string uniqueID;

    private int unreadItemNotifications;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void Update()
    {
        UpdateTime();
        CheckRemainingTime();
    }

    void UpdateValues()
    {
        InitializeTimer();
        UpdateTime();

        // If opened
        foreach (ItemTabContent itc in itemTabContentClaimed)
        {
            if (itc.GetUniqueID().Equals(this.GetUniqueID()))
            {
                Claimed();
            }
        }
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
        itemTabContentText.text = itemTabContentText.text.Replace("red", "black");

        itemClaimButton.interactable = false;
        itemClaimButtonImage.material = PanelManager.instance.inventoryPanel.greyScaleMaterial;

        // Decrement counter
        if (unreadItemNotifications > 0) unreadItemNotifications--;

        // Reload game
        PlayerDataStorageManager.instance.SaveThenLoad();
    }

    // We don't need this yet, maybe in the future
    void Unclaimed()
    {

    }

    void InitializeTimer()
    {
        remainingTime_ItemTabContent = DateTime.Now;

        // Not initialized before
        if (finishingTime_ItemTabContent.Equals(DateTime.MinValue))
        {
            print("Initializing timer");
            finishingTime_ItemTabContent = DateTime.Now + itemTime;
        }

        print("Current time: " + remainingTime_ItemTabContent);
        print("Remaining time: " + (finishingTime_ItemTabContent - remainingTime_ItemTabContent));
        print("Finishing time: " + finishingTime_ItemTabContent);
    }

    // Update remaining time left Text
    void UpdateTime()
    {
        if (remainingTime.Hours >= 24 && remainingTime.Days > 0)
        {
            itemTabContentText.text =
            "항목 내용\n<size=11><color=red>(" +
            remainingTime.Days +
            "일 남음)</color></size>";
        }
        else if (remainingTime.Hours >= 1 && remainingTime.Minutes > 0)
        {
            itemTabContentText.text =
            "항목 내용\n<size=11><color=red>(" +
            remainingTime.Hours +
            "시간 남음)</color></size>";
        }
        else if (remainingTime.Hours < 1 && remainingTime.Days < 0)
        {
            itemTabContentText.text =
           "항목 내용\n<size=11><color=red>(" +
           remainingTime.Hours +
           "분 남음)</color></size>";
        }
    }

    // Check if time is exhausted before claiming
    void CheckRemainingTime()
    {
        if (itemTime.Equals(TimeSpan.Zero))
        {
            PanelManager.instance.inventoryPanel.RemoveItem(this);
        }
        // Subtract
        else
        {
            remainingTime_ItemTabContent = DateTime.Now;
            remainingTime = finishingTime_ItemTabContent - remainingTime_ItemTabContent;
        }
    }

    public void ClaimItem()
    {
        Claimed();
        
        // Update some item variables here using the data.localVariable call in the SaveData function below
        // for example
        // int numberOfHammers = 0;
        // numberOfHammers++;
        // data.numberOfHammers = numberOfHammers;
    }

    public void SetUniqueID(string _uniqueID) => uniqueID = _uniqueID;

    public string GetUniqueID() { return (itemTabContentText.text).GetHashCode().ToString(); }

    public void LoadData(PlayerDataManager data)
    {
        remainingTime_ItemTabContent = PlayerDataStorageManager.instance.StringToDateTime
            (data.remainingTime_ItemTabContent[transform.GetSiblingIndex()]);

        itemTabContent = data.itemTabContent;
        itemTabContentClaimed = data.itemTabContentClaimed;
        unreadItemNotifications = data.unreadNotificationsInventoryPanel_ItemTab;
    }

    public void SaveData(ref PlayerDataManager data)
    {
        data.remainingTime_ItemTabContent.Insert(transform.GetSiblingIndex(), remainingTime_ItemTabContent.ToString());

        data.itemTabContent = itemTabContent;
        data.itemTabContentClaimed = itemTabContentClaimed;
        data.unreadNotificationsInventoryPanel_ItemTab = unreadItemNotifications;
    }
}
