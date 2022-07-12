using UnityEngine;
using UnityEngine.UI;

public class InventoryMailPopup : MonoBehaviour
{
    [SerializeField] Text inventoryMailText;

    public void UpdateMailPopup(string mailText)
    {
        inventoryMailText.text = mailText;
    }
}
