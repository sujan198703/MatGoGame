using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossLimitPopup : MonoBehaviour
{
    public void ChangeLossLimit()
    {
        PopupManager.instance.lossLimitChangeCompletedPopup.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
