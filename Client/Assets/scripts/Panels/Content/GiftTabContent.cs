using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GiftTabContent : MonoBehaviour, PlayerDataStorageInterface
{
    public Text giftTabContentText;
    public Text giftTabContentValueText;
    public Button giftTabContentClaimButton;
    public Image giftTabContentProgressBar;

    void Awake() => PlayerDataStorageManager.instance.AddToDataStorageObjects(this);

    void Start() => UpdateValues();

    void UpdateValues()
    {

    }

    public void ClaimGift()
    {
        Claimed();


    }

    void Claimed()
    {
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.material = PanelManager.instance.inventoryPanel.greyScaleMaterial;
        }

        giftTabContentClaimButton.interactable = false;
    }

    void Unclaimed()
    {
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.material = null;
        }

        giftTabContentClaimButton.interactable = true;
    }

    public void LoadData(PlayerDataManager data)
    {
    }

    public void SaveData(ref PlayerDataManager data)
    {
    }
}
