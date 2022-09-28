using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using FreeNet;

public class CPopupGoStop : MonoBehaviour {

	int goCounter; // track the amount of times go was pressed

	void Awake()
	{
		transform.Find("button_go").GetComponent<Button>().onClick.AddListener(this.on_touch_01);
		transform.Find("button_stop").GetComponent<Button>().onClick.AddListener(this.on_touch_02);
	}

	// Go
	void on_touch_01()
	{
		on_choice_go_or_stop(1);

		PlayerPrefs.SetInt("PlayerFirstScore", PlayerPrefs.GetInt("PlayerFirstScore") + 1);
		goCounter++;

		// Double score 
		if (goCounter.Equals(3))
        {
			PlayerPrefs.SetInt("PlayerFirstScore", PlayerPrefs.GetInt("PlayerFirstScore") * 2);
        }
	}

	// Stop
	void on_touch_02()
	{
		on_choice_go_or_stop(0);
	}

	
	void on_choice_go_or_stop(byte is_go)
	{
		gameObject.SetActive(false);

		CPacket choose_msg = CPacket.create((short)PROTOCOL.ANSWER_GO_OR_STOP);
		choose_msg.push(is_go);
		CNetworkManager.Instance.send(choose_msg);
	}
}
