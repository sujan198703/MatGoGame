using UnityEngine;
using System.Collections;

public class CPlayerInfoSlot : MonoBehaviour {

	bool is_player_turn;
	public GameObject[] go;
	public GameObject[] shake;
	public GameObject[] ppuk;
	public GameObject[] pee;

	public GameObject clock;
	public UnityEngine.UI.Text name_text;
	public UnityEngine.UI.Text money_text;
	public UnityEngine.UI.Text score_text;
	public GameObject order_mark;
	public SpriteRenderer user_profile_image;
	private Coroutine counting;

	void Awake()
	{
		is_player_turn = true;
		reset();
		//this.score_text = gameObject.transform.Find("score").GetComponent<TextMesh>();
		// this.go_text = gameObject.transform.Find("go").GetComponent<TextMesh>();
		// this.shake_text = gameObject.transform.Find("shake").GetComponent<TextMesh>();
		// this.ppuk_text = gameObject.transform.Find("ppuk").GetComponent<TextMesh>();
		// this.pee_count_text = gameObject.transform.Find("pee").GetComponent<TextMesh>();
	}

	public void reset(){
		set_order_mark(false);
		is_player_turn = false;
		clock.SetActive(false);
		name_text.text ="";
		money_text.text = "";
		for(int i = 0; i < 3; i++){
			if(i == 0){
				go[i].SetActive(true);
				shake[i].SetActive(true);
				ppuk[i].SetActive(true);
			}else{
				go[i].SetActive(false);
				shake[i].SetActive(false);
				ppuk[i].SetActive(false);
			}
		}
	}

	public void set_player_turn(bool yes){
		if(yes){
			counting = StartCoroutine(time_counting());
			clock.SetActive(true);
		}else{
			if(counting != null){
				StopCoroutine(counting);
			}
			clock.SetActive(false);
		}	

	}

	IEnumerator time_counting(){
		for(int i = GameSetting.instance.turn_time; i > -1 ; i--){
			this.clock.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text = i.ToString();
			yield return new WaitForSeconds(1f);
		}
		this.clock.GetComponent<Animator>().SetBool("alarm", true);
	}
	public void set_user_info(string name, string money, string profile_image){
		if(name == "bot"){
			gameObject.transform.Find("Canvas/bot").gameObject.SetActive(true);
			gameObject.transform.Find("Canvas/player").gameObject.SetActive(false);
		}else{
			gameObject.transform.Find("Canvas/bot").gameObject.SetActive(false);
			gameObject.transform.Find("Canvas/player").gameObject.SetActive(true);
			this.name_text.text = name;
			this.money_text.text = money;
		}
		
	}

	public void update_score(short score)
	{
		if(score > 50)
			Debug.Log("");
		this.score_text.text = score.ToString();
	}

	public void set_order_mark(bool yes){
		if(yes)
			this.order_mark.SetActive(true);
		else	
			this.order_mark.SetActive(false);
	}

	public void update_go(short go)
	{
		if(go <= 0)
			return;
		this.go[2].GetComponent<UnityEngine.UI.Text>().text = go.ToString();
		this.go[0].SetActive(false);
		this.go[1].SetActive(true);
		this.go[2].SetActive(true);
	}

	public void update_shake(short shake)
	{
		if(shake <= 0)
			return;
		this.shake[2].GetComponent<UnityEngine.UI.Text>().text = shake.ToString();
		this.shake[0].SetActive(false);
		this.shake[1].SetActive(true);
		this.shake[2].SetActive(true);
	}


	public void update_ppuk(short ppuk)
	{
		if(ppuk <= 0)
			return;
		this.ppuk[2].GetComponent<UnityEngine.UI.Text>().text = ppuk.ToString();
		this.ppuk[0].SetActive(false);
		this.ppuk[1].SetActive(true);
		this.ppuk[2].SetActive(true);
	}

	
	public void update_peecount(byte count)
	{

       // if (PlayerTurn)
        //{
		//	PlayerPrefs.SetInt("PlayerFirstScore", count);
		//	PlayerTurn = false;
        //}
        //else
        //{
		//	PlayerPrefs.SetInt("PlayerSecondScore", count);
		//	PlayerTurn = true;
		//}

		//Debug.Log("Blood Score in"+string.Format("({0})", count));
		//this.pee_count_text.text = string.Format("({0})", count);
	}
}
