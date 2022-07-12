using UnityEngine;

public class PurchaseFailurePopup : MonoBehaviour
{
    public void Enable() => gameObject.SetActive(true);

    public void Disable() => gameObject.SetActive(false);
}
