using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel_NameChange : MonoBehaviour
{
    [SerializeField] private GameObject nameNotValid;
    [SerializeField] private TextMeshProUGUI nameNotValidText;
    [SerializeField] private Button confirmButton;


    string[] forbiddenWords = { "badword", "나쁜 말" };

    public void UpdateName(TMP_Text nameField)
    {
        // Check for spaces and special characters
        if (NameIsInvalid(nameField.text.ToLower()))
        {
            ShowNameNotValid("닉네임이 형식이나 규칙을 따르지 않습니다."); // Nickname does not conform to format or convention.
            return;
        }
        // Check if greater than 2 characters
        if (NameLessThanTwo(nameField.text.ToLower()))
        {
            ShowNameNotValid("2자 이상의 닉네임을 입력하세요."); // Please enter a nickname of at least 2 characters.
            return;
        }
        // Check if less than 12 characters
        if (NameGreaterThanTwelve(nameField.text.ToLower()))
        {
            ShowNameNotValid("닉네임은 12자 이내로 입력해주세요"); // Please enter a nickname of 12 characters or less.
            return;
        }
        // Check if previously changed within 24 hours
        if (NamePreviouslyChanged(nameField.text.ToLower()))
        {
            ShowNameNotValid("닉네임은 하루에 한 번만 변경할 수 있습니다."); // You can only change your nickname once per day.
            return;
        }
        // Check if already exists
        if (NameInUse(nameField.text.ToLower()))
        {
            ShowNameNotValid("별칭이 사용 중입니다."); // The alias in use.
            return;
        }
        // Check for forbidden words
        for (int i = 0 ; i < forbiddenWords.Length ; i++)
        {
            if (nameField.text.ToLower().Contains(forbiddenWords[i].ToLower()))
            {
                ShowNameNotValid("금지된 단어가 포함되어 있습니다."); // Contains forbidden words
                return;
            }
        }

        // If all okay, make button interactable
        HideNameNotValid();
        confirmButton.interactable = true;
    }

    public void ChangeName(TMP_Text nameField)
    {
        PlayerDataStorageManager.instance.playerDataManager.playerName = nameField.text;
    }

    // CHECK FUNCTIONS
    // Check if username has invalid characters
    bool NameIsInvalid(string userName)
    {
        Regex RgxUrl = new Regex("[^a-z0-9]");
        if (!RgxUrl.IsMatch(userName)) return true;

        return false;
    }

    // Check if username has less than 2 characters
    bool NameLessThanTwo(string userName)
    {
        if (userName.Length < 2) return true;
        else return false;
    }

    // Check if username has greater than 12 characters
    bool NameGreaterThanTwelve(string userName)
    {
        if (userName.Length > 12) return true;
        else return false;
    }

    // Check if name was previously changed in 24 hours
    bool NamePreviouslyChanged(string userName)
    {
        TimeSpan oneDay = new TimeSpan(24, 0, 0);
        TimeSpan currentTime;
        currentTime = DateTime.UtcNow - PlayerDataStorageManager.instance.playerDataManager.profileNameChangeTime;

        if (currentTime >= oneDay) return false;
        
        return true;
    }

    // Check server DB if a nickname already exists
    bool NameInUse(string userName)
    {
        string[] allNamesInDatabase = { "INUSE", "APPLES" }; // replace with array from database
        foreach (string name in allNamesInDatabase)
        {
            if (name.Equals(userName))
            {
                return true;
            }
        }
        return false;
    }

    void ShowNameNotValid(string popupText)
    {
        confirmButton.interactable = false;
        nameNotValid.SetActive(true);
        nameNotValidText.text = popupText;
    }

    void HideNameNotValid()
    {
        nameNotValid.SetActive(false);
    }
}
