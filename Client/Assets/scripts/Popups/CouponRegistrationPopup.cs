using UnityEngine;
using TMPro;

public class CouponRegistrationPopup : MonoBehaviour
{
    [SerializeField] GameObject couponRegistrationInput;
    [SerializeField] GameObject couponRegistrationComplete;
    [SerializeField] GameObject couponRegistrationFailed;
    [SerializeField] TMP_Text couponRegistrationInputField;

    public void Confirm()
    {
        // First click
        if (couponRegistrationInput.activeInHierarchy)
        {
            if (ValidCoupon(couponRegistrationInputField.text))
            {
                couponRegistrationInput.SetActive(false);
                couponRegistrationComplete.SetActive(true);
            }
            else
            {
                couponRegistrationInput.SetActive(false);
                couponRegistrationFailed.SetActive(true);
            }
        }
        // Second click
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    bool ValidCoupon(string couponRegistrationNumber)
    {
        // Subtract from web server
        if (true)
        {
            return true;
        }
        // Invalid
        else
        {
            return false;
        }
    }
}
