using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [Header("Popups")]
    public AccountInfoPopup accountInfoPopup;
    public CouponRegistrationPopup couponRegistrationPopup;
    public FreeRefillPaymentPopup freeRefillPaymentPopup;
    public FreeRefillPaymentCompletedPopup freeRefillPaymentCompletedPopup;
    public Friends_GetInvolvedPopup friends_GetInvolvedPopup;
    public Friends_RoomMakingPopup friends_RoomMakingPopup;
    public GameMoneyLimitPopup gameMoneyLimitPopup;
    public IncorrectPasswordPopup incorrectPasswordPopup;
    public InventoryMailPopup inventoryMailPopup;
    public LogoutReconfirmationPopup logoutReconfirmationPopup;
    public LossLimitCannotBeChangedPopup lossLimitCannotBeChangedPopup;
    public LossLimitChangeCompletedPopup lossLimitChangeCompletedPopup;
    public LossLimitChangeNoticePopup lossLimitedChangeNoticePopup;
    public LossLimitPopup lossLimitPopup;
    public ProfileAvatarChangePopup profileAvatarChangePopup;
    public ProfileNameChangePopup profileNameChangePopup;
    public PurchaseFailurePopup purchaseFailurePopup;
    public PurchaseIAPCompletedPopup purchaseIAPCompletePopup;
    public PurchaseShopCompletedPopup purchaseShopCompletedPopup;
    public PurchaseConfirmationPopup purchaseConfirmationPopup;


    // Static Variables
    private static PopupManager _instance;

    public static PopupManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

}
