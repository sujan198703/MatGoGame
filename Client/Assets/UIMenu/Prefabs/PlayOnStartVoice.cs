using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayOnStartVoice : MonoBehaviour
{


	GameObject beforeplay;
	GameObject beforeplay1;
	GameObject beforeplay2;
	GameObject beforeSun;
	float times = 0f;
	float timer = 2.0f;

	AudioSource playonclick;

	// Start is called before the first frame update
	void Start()
	{
		beforeSun = GameObject.Find("BeforeSun");

		beforeplay = GameObject.Find("beforeplay");
		beforeplay1 = GameObject.Find("beforeplay1");
		beforeplay2 = GameObject.Find("beforeplay2");

		//sSplayonclick = GetComponent<AudioSource>();
	}


	// Update is called once per frame
	void Update()
    {

		//StartCoroutine(ExampleCoroutine());
	//	times += Time.deltaTime;


	}

	public void PlayRandomOnClick()
    {

		

		int rand = Random.Range(0, 3);

        if (rand == 1 )
        {
			beforeplay.GetComponent<AudioSource>().Play();

		}else if (rand == 2)
        {

			beforeplay1.GetComponent<AudioSource>().Play();

        }
        else
        {
			beforeplay2.GetComponent<AudioSource>().Play();

		}



	}

	


}
