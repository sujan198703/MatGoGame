using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPanelContent : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text nyangAmountText;
    [SerializeField] private Text peopleOnlineText;
    [SerializeField] private Image lobbyPanelContentImage;
    [SerializeField] private GameObject lobbyPanelContentLock;
    [SerializeField] private GameObject lobbyPanelContentStartButton;
    [SerializeField] private GameObject lobbyPanelContentUnlockableButton;

    [SerializeField] Color32 originalTitleColor;
    [SerializeField] Color32 originaNyangAmountColor;
    [SerializeField] Color32 originalPeopleOnlineColor;

    public void Locked()
    {   
        titleText.color = new Color32(20, 20, 20, 255);
        nyangAmountText.color = new Color32(220, 220, 220, 255);
        peopleOnlineText.color = new Color32(20, 20, 20, 255);

        lobbyPanelContentImage.material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        lobbyPanelContentLock.GetComponent<Image>().material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        lobbyPanelContentStartButton.GetComponent<Image>().material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        lobbyPanelContentUnlockableButton.GetComponent<Image>().material = PanelManager.instance.inventoryPanel.greyScaleMaterial;

        lobbyPanelContentLock.SetActive(true);
        lobbyPanelContentStartButton.SetActive(false);
        lobbyPanelContentUnlockableButton.SetActive(false);
    }

    public void Unlockable()
    {
        titleText.color = originalTitleColor;
        nyangAmountText.color = originaNyangAmountColor;
        peopleOnlineText.color = originalPeopleOnlineColor;

        lobbyPanelContentImage.material = null;
        lobbyPanelContentLock.GetComponent<Image>().material = null;
        lobbyPanelContentStartButton.GetComponent<Image>().material = null;
        lobbyPanelContentUnlockableButton.GetComponent<Image>().material = null;

        lobbyPanelContentLock.SetActive(false);
        lobbyPanelContentStartButton.SetActive(false);
        lobbyPanelContentUnlockableButton.SetActive(true);
    }

    public void Unlocked()
    {
        titleText.color = originalTitleColor;
        nyangAmountText.color = originaNyangAmountColor;
        peopleOnlineText.color = originalPeopleOnlineColor;

        lobbyPanelContentImage.material = null;
        lobbyPanelContentLock.GetComponent<Image>().material = null;
        lobbyPanelContentStartButton.GetComponent<Image>().material = null;
        lobbyPanelContentUnlockableButton.GetComponent<Image>().material = null;

        lobbyPanelContentLock.SetActive(false);
        lobbyPanelContentStartButton.SetActive(true);
        lobbyPanelContentUnlockableButton.SetActive(false);
    }
}
