using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_MODE
{
    MULTI = 0,
    PLAY_WITH_AI = 1,
    TRANSITION = 2,
    NONE = 3
}

public enum GLOBAL_EVENT
{
    GOOUT = 0
}
public class GameController : MonoBehaviour
{
    public delegate void GlobalEvent(GLOBAL_EVENT status);
	public GlobalEvent global_event;
    public GameObject game_room_slot;
    static public GameController instance;
    private GAME_MODE game_mode;
    public bool IsAIMode = false;
    // Start is called before the first frame update
    void Start()
    {
        SetGameMode(GAME_MODE.PLAY_WITH_AI);
    }
    void Awake()
    {
        if (instance == null) instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
       
    }
 
    public void ShowLoginPage(){
        CUIManager.Instance.clear();
        CUIManager.Instance.show(UI_PAGE.LOGIN);
    }
    
    public void SetServerIP(string ip){
        Backend.instance.serverURL = "ws://" + ip + ":8080";
    }
    public void ShowStagePage(){
        ClearUI();
        CUIManager.Instance.show(UI_PAGE.STAGE_SELECT);
    }
    public void ShowGameRoomPage(){
        GameObject game_room = GameObject.Instantiate(Resources.Load("playroom")) as GameObject;
        game_room.transform.SetParent(game_room_slot.transform);
    }
    public void DeleteGameRoomPage(){
        try{
            Destroy(GameObject.Find("playroom(Clone)"));
        }catch{}
    }
    public void StartFind(){
        if(IsAIMode)
            NotFoundOpponent();
        else
            Backend.OnOpen();
        ClearUI();
        
        CUIManager.Instance.show(UI_PAGE.POPUP_STATE, "Finding Opponent...");
    }
    
    public void NotFoundOpponent(){
        float delay_time = 1.0f;
        CUIManager.Instance.show(UI_PAGE.POPUP_STATE, "No Match found. You play with AI.", delay_time);
        StartCoroutine(StartWithAI(delay_time));
    }
    
    IEnumerator StartWithAI(float delay){
        SetGameMode(GAME_MODE.TRANSITION);
        yield return new WaitForSeconds(delay);
        SetGameMode(GAME_MODE.PLAY_WITH_AI);
        ClearUI();
        ShowGameRoomPage();
        CPlayRoomUI.Instance.start_game();
    }
    
    public void StartMultiGame(){
        SetGameMode(GAME_MODE.MULTI);
        ClearUI();
        ShowGameRoomPage();
        CPlayRoomUI.Instance.start_game();
    }

    public void MatchedOtherPlayer(){
        CUIManager.Instance.clear();
        ShowGameRoomPage();
        StartMultiGame();
    }
    
    public void SetGameMode(GAME_MODE mode){
        this.game_mode = mode;
    }

    public bool IsMulti(){
        return this.game_mode == GAME_MODE.MULTI;
    }

    public bool IsAI(){
        return this.game_mode == GAME_MODE.PLAY_WITH_AI;
    }
    private void ClearUI(){
        DeleteGameRoomPage();
        CUIManager.Instance.clear();
    }
    public void GoOut(){
        SetGameMode(GAME_MODE.NONE);
        ClearUI();
        CUIManager.Instance.show(UI_PAGE.STAGE_SELECT);
        if(global_event != null)
            global_event(GLOBAL_EVENT.GOOUT);
    }
}
