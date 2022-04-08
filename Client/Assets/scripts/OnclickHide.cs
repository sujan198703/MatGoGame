using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnclickHide : MonoBehaviour
{

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
        Debug.Log("clicked ");
        HintPanel.SetActive(false);
    }
}
