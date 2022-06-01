using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{

    public Text PlayerOne;
    public Text PlayerTwo;
    public  int PlayerOneScore = 0;
    public  int PlayerTwoScore = 0;
    string FristPlayer = "PlayerFirstScore";
    string SecondPlayer = "PlayerSecondScore";
    bool firstrun = true;

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("First start ");
       // PlayerTwoScore += PlayerPrefs.GetInt(OldScoreKey, PlayerNewScore);

       PlayerTwo.text = PlayerTwoScore.ToString();
       PlayerOne.text = PlayerOneScore.ToString();
  
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
