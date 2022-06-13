using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckifGameObject : MonoBehaviour
{

    int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (GameObject.Find("hwatoo(Clone)"))
        {

            count=GameObject.FindGameObjectsWithTag("card").Length;
           // Debug.Log("It exists"+ count);
        }


    }
}
