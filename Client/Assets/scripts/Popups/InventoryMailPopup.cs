using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryMailPopup : MonoBehaviour
{
    [SerializeField] TMP_Text inventoryMailText;

    private int mailIndex;
    private MailTabContent mailTabContent;

    public void UpdateMailPopupContentText(string mailText)
    {
        inventoryMailText.text = mailText;
    }

    public void UpdateMailTabContentObject(MailTabContent mailTabContent)
    {
        this.mailTabContent = mailTabContent;
    }

    public void UpdateMailIndex(int mailIndex) => this.mailIndex = mailIndex;

    public void DeleteMail()
    {
        PanelManager.instance.inventoryPanel.RemoveMail(mailTabContent);
    }

}
