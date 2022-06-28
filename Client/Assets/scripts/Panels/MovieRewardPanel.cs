using UnityEngine;
using UnityEngine.UI;

public class MovieRewardPanel : MonoBehaviour
{
    public Text nyangAdsWatchedText;
    public Text chipAdsWatchedText;
    public GameObject movieRewardLotteryTicket;
    

    public void RequestAndLoadRewardedAd(int rewardedVideoIndex)
    {
        AdManager.instance.RequestAndLoadRewardedAd(rewardedVideoIndex);
        AdManager.instance.ShowRewardedAd(rewardedVideoIndex);
    }

    public void AddNyang()
    {
        // Update Nyang
        PlayerDataStorageManager.instance.playerDataManager.nyangsPocket += 1000;

        // Update Nyang Counter
        if (PlayerDataStorageManager.instance.playerDataManager.nyangAdsWatched < 10)
            PlayerDataStorageManager.instance.playerDataManager.nyangAdsWatched++;

        // Update Nyang Text
        nyangAdsWatchedText.text = "오늘 시청 <color=red>" + PlayerDataStorageManager.instance.playerDataManager.nyangAdsWatched + "</color>/10회";
    }


    public void AddChip()
    {
        // Update Chip
        PlayerDataStorageManager.instance.playerDataManager.matgoChips += 1000;

        // Update Chip
        if (PlayerDataStorageManager.instance.playerDataManager.chipAdsWatched < 10)
            PlayerDataStorageManager.instance.playerDataManager.chipAdsWatched++;

        // Update Chip Text
        chipAdsWatchedText.text = "오늘 시청 <color=red>" + PlayerDataStorageManager.instance.playerDataManager.chipAdsWatched + "</color>/10회";
    }
}
