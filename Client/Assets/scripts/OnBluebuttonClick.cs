using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBluebuttonClick : MonoBehaviour
{

    public GameObject HintPanel;
    bool check = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void buttonclick()
    {

        if (check)
        {
            Debug.Log("clicked ");
            HintPanel.SetActive(true);
            check = false;
        }
        else if (!check){
            Debug.Log("clicked ");
            HintPanel.SetActive(false);
            check = true;
        }
       
    }
}
