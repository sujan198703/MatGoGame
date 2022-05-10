using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{

    public Text ScoreText;
    public Text OverallScore;
    public  int PlayerNewScore = 0;
    public  int PlayerOldScore = 0;
    string OldScoreKey = "PlayerOldScore";
    string NewScoreKey = "PlayerNewScore";
    bool firstrun = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerOldScore+=PlayerPrefs.GetInt(OldScoreKey, PlayerNewScore);

        OverallScore.text = PlayerOldScore.ToString();
       PlayerPrefs.SetInt(NewScoreKey, 0);
      // PlayerPrefs.SetInt(OldScoreKey, PlayerNewScore);

        ScoreText.text = "0";
       // PlayerOldScore = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // PlayerOldScore += PlayerPrefs.GetInt(OldScoreKey);
  

        //getting new score on runtime 
          PlayerNewScore = PlayerPrefs.GetInt(NewScoreKey);

        //new score being added on old score
        PlayerOldScore += PlayerNewScore;

        //setting new score on oldscore
        PlayerPrefs.SetInt(OldScoreKey, PlayerOldScore);


       //Showing New Score on UI
        ScoreText.text = PlayerNewScore.ToString();

        //showing addtion of new score on Old Score
        OverallScore.text = PlayerNewScore.ToString();

        PlayerPrefs.Save();



    }



}
