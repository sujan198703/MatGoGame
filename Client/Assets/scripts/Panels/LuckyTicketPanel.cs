using UnityEngine;
using UnityEngine.UI;

public class LuckyTicketPanel : MonoBehaviour
{
    [SerializeField] private Text luckyPanelText;

    void Start() => UpdateText();

    void UpdateText()
    {
        // Timer condition for lucky timer
        if (false)
        {
            this.gameObject.SetActive(true);
            luckyPanelText.text = "당첨을 축하합니다.\n<size=28>20</size>만냥";
        }
        else
            gameObject.SetActive(false);
    }
}
