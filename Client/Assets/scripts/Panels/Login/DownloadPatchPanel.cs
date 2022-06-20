using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DownloadPatchPanel : MonoBehaviour
{
    public Text notificationText;
    public Slider downloadProgressBar;

    string downloadingContent = "컨텐츠 다운로드 중";
    string downloadingComplete = "다운로드 완료";

    public bool PatchAvailable()
    {
        // Check from server if crash patch available
        return false;

        // If patch available
        // Update notification text while download
        if (false) notificationText.text = downloadingContent;

    }


    void DownloadCompleted()
    {
    }

    public void ShowDownloadPatchPanel()
    {
        gameObject.SetActive(true);
    }
}
