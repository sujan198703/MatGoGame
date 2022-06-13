using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject PopupRef;
    private void Awake() => HidePopUp();

    public void ShowPopUp()
    {
        PopupRef.SetActive(true);
    }

    public void HidePopUp()
    {
        if (!PopupRef.activeInHierarchy == true)
            return;

        PopupRef.SetActive(false);
    }
}
