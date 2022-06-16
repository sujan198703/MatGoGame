﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum UI_PAGE
{
	PLAY_ROOM,
	POPUP_PLAYER_ORDER,
	POPUP_CHOICE_CARD,
	POPUP_GO_STOP,
	POPUP_ASK_SHAKING,
	POPUP_SHAKING_CARDS,
	POPUP_ASK_KOOKJIN,
	POPUP_GAME_RESULT,
	POPUP_GAME_RESULT_LOST,
	POPUP_GO_COUNT,
	POPUP_STOP,
	POPUP_FIRST_PLAYER,
	GAME_OPTION,
	MAIN_MENU,
	CREDIT_BAR,
	STAGE_SELECT,
	LOGIN,
	POPUP_STATE
}

public class CUIManager : CSingletonMonobehaviour<CUIManager>
{
	Dictionary<UI_PAGE, GameObject> ui_objects;

	void Awake()
	{
		this.ui_objects = new Dictionary<UI_PAGE, GameObject>();
		this.ui_objects.Add(UI_PAGE.MAIN_MENU, transform.Find("main_menu").gameObject);
		this.ui_objects.Add(UI_PAGE.CREDIT_BAR, transform.Find("credit").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_PLAYER_ORDER, transform.Find("popup_player_order").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_CHOICE_CARD, transform.Find("popup_choice_card").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_GO_STOP, transform.Find("popup_gostop").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_ASK_SHAKING, transform.Find("popup_shaking").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_SHAKING_CARDS, transform.Find("popup_shaking_cards").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_ASK_KOOKJIN, transform.Find("popup_kookjin").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_GAME_RESULT, transform.Find("popup_result").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_GAME_RESULT_LOST, transform.Find("popup_lost").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_GO_COUNT, transform.Find("popup_go").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_STOP, transform.Find("popup_stop").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_FIRST_PLAYER, transform.Find("popup_first_player").gameObject);
		this.ui_objects.Add(UI_PAGE.GAME_OPTION, transform.Find("option_cardhint").gameObject);
		this.ui_objects.Add(UI_PAGE.STAGE_SELECT, transform.Find("stage").gameObject);
		this.ui_objects.Add(UI_PAGE.LOGIN, transform.Find("Login").gameObject);
		this.ui_objects.Add(UI_PAGE.POPUP_STATE, transform.Find("popup_state").gameObject);
	}


	public GameObject get_uipage(UI_PAGE page)
	{
		return this.ui_objects[page];
	}


	public void show(UI_PAGE page)
	{
		this.ui_objects[page].SetActive(true);
	}

	public void show(UI_PAGE page, string msg){
		this.ui_objects[page].SetActive(true);
		this.ui_objects[page].transform.Find("msg_txt").GetComponent<UnityEngine.UI.Text>().text = msg;
	}

	public void show(UI_PAGE page, string msg, float delay){
		this.ui_objects[page].SetActive(true);
		this.ui_objects[page].transform.Find("msg_txt").GetComponent<UnityEngine.UI.Text>().text = msg;
		StartCoroutine(delayHide(page, delay));
	}

	public void hide(UI_PAGE page)
	{
		this.ui_objects[page].SetActive(false);
	}

	public void clear(){
		foreach(KeyValuePair<UI_PAGE, GameObject> ui in ui_objects){
			hide(ui.Key);
		}
	}

	IEnumerator delayHide(UI_PAGE page, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.ui_objects[page].SetActive(false);
	}
}
