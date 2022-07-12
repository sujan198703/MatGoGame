using UnityEngine;
using UnityEngine.UI;

public class PurchaseIAPCompletedPopup : MonoBehaviour
{
    [SerializeField] Text IAPPurchaseText;

    public void UpdateValues(int IAPAmount) => IAPPurchaseText.text = IAPAmount.ToString() + "루비";

}
