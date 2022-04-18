using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnclickHide : MonoBehaviour
{
    bool check = true;
    public GameObject HintPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPanelclick()
    {

        if (check)
        {
            Debug.Log("clicked ");
            HintPanel.SetActive(false);
            check = false;
        }else if (!check)
        {
            Debug.Log("clicked ");
            HintPanel.SetActive(true);
            check = true;
        }
        
    }
}
