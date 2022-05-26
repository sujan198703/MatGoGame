﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FreeNet;

public class CPopupGameResult : MonoBehaviour {

	Sprite win_sprite;
	Sprite lose_sprite;

	Image win_lose;
	Text money;
	Text score;
	Text double_val;
	Text final_score;
	GameObject succes;
	GameObject failure;
	AudioSource playsound;



    public void Start()
    {
		succes = GameObject.Find("success");
		failure = GameObject.Find("failure");

		playsound = GetComponent<AudioSource>();
		succes.GetComponent<AudioSource>();
		failure.GetComponent<AudioSource>();

	}
	void Awake()
	{
		this.win_sprite = CSpriteManager.Instance.get_sprite("win");
		this.lose_sprite = CSpriteManager.Instance.get_sprite("lose");

		transform.Find("button_play").GetComponent<Button>().onClick.AddListener(this.on_touch);
		this.win_lose = transform.Find("title").GetComponent<Image>();
		this.money = transform.Find("money").GetComponent<Text>();
		this.score = transform.Find("score").GetComponent<Text>();
		this.double_val = transform.Find("double").GetComponent<Text>();
		this.final_score = transform.Find("final_score").GetComponent<Text>();
	}


	void on_touch()
	{
		PlayerPrefs.SetInt("PlayerNewScore", 0);
		CUIManager.Instance.hide(UI_PAGE.POPUP_GAME_RESULT);

		CPacket send = CPacket.create((short)PROTOCOL.READY_TO_START);
		CNetworkManager.Instance.send(send);
	}


	public void refresh(byte is_win,
		int money,
		int score,
		int double_val,
		int final_score)
	{
		if (is_win == 1)
		{
			Debug.Log("Winner");
			this.win_lose.sprite = this.win_sprite;
			succes.GetComponent<AudioSource>().Play();

		}
		else
		{
			Debug.Log("Loser");
			failure.GetComponent<AudioSource>().Play();

			this.win_lose.sprite = this.lose_sprite;
		}

		this.money.text = money.ToString();
		this.score.text = score.ToString();
		this.double_val.text = double_val.ToString();
		this.final_score.text = final_score.ToString();
	}
}
