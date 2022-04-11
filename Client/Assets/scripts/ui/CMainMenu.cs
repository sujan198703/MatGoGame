using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CMainMenu : MonoBehaviour
{
	GameObject room;
	GameObject ef_intro;

	void Awake()
	{
		this.room = GameObject.Find("Main").transform.Find("playroom").gameObject;
		transform.Find("button_play").GetComponent<Button>().onClick.AddListener(this.on_play);

		this.ef_intro = GameObject.Find("ef_falling");

		CTableDataManager.Instance.load_all();

		Invoke("on_play", 0.001f);
	}


	void on_play()
	{
		//this.room.SetActive(true);
		//this.ef_intro.SetActive(false);
		//CUIManager.Instance.hide(UI_PAGE.CREDIT_BAR);
		//CUIManager.Instance.hide(UI_PAGE.MAIN_MENU);
		CUIManager.Instance.show(UI_PAGE.STAGE_SELECT);
		CUIManager.Instance.get_uipage(UI_PAGE.STAGE_SELECT).GetComponent<CStageSelect>().show();
	}
}
