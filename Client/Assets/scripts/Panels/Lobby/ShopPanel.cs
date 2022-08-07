using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour, PlayerDataStorageInterface
{
    [SerializeField] Texture2D buttonSelected;
    [SerializeField] Texture2D buttonUnselected;

    [SerializeField] private GameObject eventPanel, rubyPanel, characterPanel, itemPanel, memberPanel;
    [SerializeField] private GameObject eventButton, rubyButton, characterButton, itemButton, memberButton;

    [HideInInspector] public int unreadNotifications;

    private string previousPanel = "CharacterPanel"; // Character panel is default

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => EnablePanel(previousPanel);

    public void EventButton() => EnablePanel("EventPanel");

    public void RubyButton() => EnablePanel("RubyPanel");

    public void ItemButton() => EnablePanel("ItemPanel");

    public void CharacterButton() => EnablePanel("CharacterPanel");

    public void MemberButton() => EnablePanel("MemberPanel");

    void EnablePanel(string panelName)
    {
        switch(panelName)
        {
            case "EventPanel":
                if (eventPanel != null) eventPanel.SetActive(true);
                break;
            case "RubyPanel":
                if (rubyPanel != null) rubyPanel.SetActive(true);
                break;
            case "CharacterPanel":
                if (characterPanel != null ) characterPanel.SetActive(true);
                break;
            case "ItemPanel":
                if (itemPanel != null) itemPanel.SetActive(true);
                break;
            case "MemberPanel":
                if (memberPanel != null) memberPanel.SetActive(true);
                break;
        }

        // Update button image
        UpdateButtonImage(panelName, true);

        if (!panelName.Equals(previousPanel))
        {
            // Disable previous panel
            DisablePreviousPanel();

            // Update previous button image
            UpdateButtonImage(previousPanel, false);
        }

        // Update previous panel name
        previousPanel = panelName;
    }

    void UpdateButtonImage(string panelName, bool selected)
    {
        switch (panelName)
        {
            case "EventPanel":
                if (selected)
                {
                    eventButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonSelected,
                    new Rect(0.0f, 0.0f, buttonSelected.width, buttonSelected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                else
                {
                    eventButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonUnselected,
                    new Rect(0.0f, 0.0f, buttonUnselected.width, buttonUnselected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                break;
            case "RubyPanel":
                if (selected)
                {
                    rubyButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonSelected,
                    new Rect(0.0f, 0.0f, buttonSelected.width, buttonSelected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                else
                {
                    rubyButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonUnselected,
                    new Rect(0.0f, 0.0f, buttonUnselected.width, buttonUnselected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                break;
            case "CharacterPanel":
                if (selected)
                {
                    characterButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonSelected,
                    new Rect(0.0f, 0.0f, buttonSelected.width, buttonSelected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                else
                {
                    characterButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonUnselected,
                    new Rect(0.0f, 0.0f, buttonUnselected.width, buttonUnselected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                break;
            case "ItemPanel":
                if (selected)
                {
                    itemButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonSelected,
                    new Rect(0.0f, 0.0f, buttonSelected.width, buttonSelected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                else
                {
                    itemButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonUnselected,
                    new Rect(0.0f, 0.0f, buttonUnselected.width, buttonUnselected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                break;
            case "MemberPanel":
                if (selected)
                {
                    memberButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonSelected,
                    new Rect(0.0f, 0.0f, buttonSelected.width, buttonSelected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                else
                {
                    memberButton.GetComponent<Image>().sprite =
                    Sprite.Create(buttonUnselected,
                    new Rect(0.0f, 0.0f, buttonUnselected.width, buttonUnselected.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                }
                break;
        }
    }
    
    void DisablePreviousPanel()
    {
        switch (previousPanel)
        {
            case "EventPanel":
                if (eventPanel != null) eventPanel.SetActive(false);
                break;
            case "RubyPanel":
                if (rubyPanel != null) rubyPanel.SetActive(false);
                break;
            case "CharacterPanel":
                if (characterPanel != null) characterPanel.SetActive(false);
                break;
            case "ItemPanel":
                if (itemPanel != null) itemPanel.SetActive(false);
                break;
            case "MemberPanel":
                if (memberPanel != null) memberPanel.SetActive(false);
                break;
        }
    }

    public void LoadData(PlayerDataManager data)
    {
        unreadNotifications = data.unreadNotificationsShopPanel;
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }


    #region PURCHASE FUNCTIONS
    public void InAppPurchase(int inAppPurchaseIndex) => InAppPurchasing.instance.InAppPurchase(inAppPurchaseIndex);

    public void PurchaseCharacter(int characterIndex)
    {
        switch (characterIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
    public void PurchaseItem(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
    #endregion
}
