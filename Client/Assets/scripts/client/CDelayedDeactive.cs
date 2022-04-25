using UnityEngine;
using System.Collections;

public class CDelayedDeactive : MonoBehaviour {

	[SerializeField]
	float delay;

	GameObject kiss;
	AudioSource playonclick;
	private void Start()
	{
		kiss = GameObject.Find("kiss");
		playonclick = GetComponent<AudioSource>();
		kiss.GetComponent<AudioSource>();

	}


	void OnEnable()
	{
		StopAllCoroutines();
		//kiss.GetComponent<AudioSource>().Play();
		StartCoroutine(delayed_deactive());
	}


	IEnumerator delayed_deactive()
	{
		yield return new WaitForSeconds(this.delay);
		gameObject.SetActive(false);
	}
}
