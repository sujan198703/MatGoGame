using UnityEngine;
using TMPro;

public class FreeRefillPaymentCompletedPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI freeRefillPaymentCompletedText;

    public void UpdateText(string text) => freeRefillPaymentCompletedText.text = text;
}
