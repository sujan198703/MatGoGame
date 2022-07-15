using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{

    public Text PlayerOne;
    public Text PlayerTwo;
    public int PlayerOneScore = 0;
    public int PlayerTwoScore = 0;
    [SerializeField] public int MoneyPackRoomOne = 1000;
    [SerializeField] public int MoneyPackRoomTwo = 10000;
    [SerializeField] public int MoneyPackRoomThree = 20000;
    [SerializeField] public int MoneyPackRoomFour = 40000;
    //KWANG is right
    //TEE is belt
    //YEOL is HEAT
    //PEE is BLOOD

    string FristPlayer = "PlayerFirstScore";
    string LightCard = "Gwang";      //light card  
    string  RedBeltCard= "HongDan";  //red blt card 
    string BoodCards = "Blood";  // Pee is Blood



    string SecondPlayer = "PlayerSecondScore";
    bool firstrun = true;

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("First start ");
       // PlayerTwoScore += PlayerPrefs.GetInt(OldScoreKey, PlayerNewScore);

       PlayerTwo.text = PlayerTwoScore.ToString();
       PlayerOne.text = PlayerOneScore.ToString();
        onGameStart();
    }


    void LoobyToMoneyReward(string RoomType)
    {

        switch (RoomType)
        {
            case "RoomOne":
                // 1000 money per point
                Debug.Log("Room 1 Selected");
                break;
            case "RoomTwo":

                Debug.Log("Room 2 Selected");
                // 10000 money per point
                break;
            case "RoomThree":

                Debug.Log("Room 3 Selected");
                // 20000 money per point
                break;
            case "RoomFour":

                Debug.Log("Room 3 Selected");
                // 30000 money per point
                break;
            default:
              
                break;
        }

    }
    public void onGameStart()
    {

        Debug.Log("The Game has started ");
        PlayerPrefs.SetInt(FristPlayer, 0);
        PlayerPrefs.SetInt(SecondPlayer, 0);
    }

    // Update is called once per frame
    void Update()
    {



        //getting Player One score on runtime 
        PlayerOneScore = PlayerPrefs.GetInt(FristPlayer);


        //getting Player Two  score on runtime 
        PlayerTwoScore = PlayerPrefs.GetInt(SecondPlayer);

      



        //Showing Player One Text on Screen
        PlayerOne.text = PlayerOneScore.ToString();

        //Showing Player two Text on Screen
        PlayerTwo.text = PlayerTwoScore.ToString();

       // PlayerPrefs.Save();



    }



}
