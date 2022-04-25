using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FreeNet;

public class CPopupPlayerOrder : MonoBehaviour {

	GameObject root;
	GameObject audio;
	 AudioSource playonclick;
	List<Transform> slots;


	void Start()
	{
		
		audio = GameObject.Find("slot02");
		playonclick = GetComponent<AudioSource>();
		audio.GetComponent<AudioSource>();
	}

	void Awake()
	{
		this.slots = new List<Transform>();

		this.root = transform.Find("root").gameObject;
		Transform slot01 = root.transform.Find("slot01");
		this.slots.Add(slot01);

		Transform slot02 = root.transform.Find("slot02");
		this.slots.Add(slot02);
	}

	


	public void reset(Sprite sprite)
	{
		for (int i = 0; i < this.slots.Count; ++i)
		{
			this.slots[i].GetComponent<Image>().sprite = sprite;
		}
	}


	public void play()
	{
		this.root.GetComponent<Animator>().Play("player_order_01");
		//Debug.Log("Hoooooo");

		//audio.GetComponent<AudioSource>().Play();



	}


	public void update_slot_info(byte slot_index, Sprite sprite)
	{
		this.slots[slot_index].GetComponent<Image>().sprite = sprite;
	}
}
