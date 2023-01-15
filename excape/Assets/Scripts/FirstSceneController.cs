using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController {
    public GuardFactory guard_factory;                               //巡逻者工厂
    public GuardActionManager action_manager;                        //运动管理器
    public int playerSign = -1;                                      //当前玩家所处哪个格子
    public GameObject player;                                        //玩家
    public UserGUI gui;                                              //交互界面

    private List<GameObject> guards;                                 //场景中巡逻者列表
    private int game_status = 0;                                  //游戏状态 0：游戏中 ； 1：游戏失败； 2：游戏胜利
    public int keynum = 0;                                          //钥匙数量           
    public bool tryget = false;                                     //正在尝试获得钥匙

    
    void Awake() {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
        guard_factory = Singleton<GuardFactory>.Instance;
        action_manager = gameObject.AddComponent<GuardActionManager>() as GuardActionManager;
        gui = gameObject.AddComponent<UserGUI>() as UserGUI;
        LoadResources();
    }

    void Update() {
        for (int i = 0; i < guards.Count; i++) {
            guards[i].gameObject.GetComponent<GuardData>().playerSign = playerSign;
        }
    }


    public void LoadResources() {
        Instantiate(Resources.Load<GameObject>("Prefabs/Road"));
        player = Instantiate(
            Resources.Load("Prefabs/Player"), 
            new Vector3(-5, 8, 28), Quaternion.identity) as GameObject;
        guards = guard_factory.GetPatrols();

        for (int i = 0; i < guards.Count; i++) {
            action_manager.GuardPatrol(guards[i]);
        }
    }

    public int GetGamestatus() {
        return game_status;
    }

    public void Restart() {
        SceneManager.LoadScene("Scenes/mySence");
    }

    void OnEnable() {
        GameEventManager.ExitChange += PlayerExit;
        GameEventManager.GameoverChange += Gameover;
        GameEventManager.GetKeyChange += getkey;
        GameEventManager.GetKeyingChange += getkeying;
    }
    void OnDisable() {
        GameEventManager.ExitChange -= PlayerExit;
        GameEventManager.GameoverChange -= Gameover;
        GameEventManager.GetKeyChange -= getkey;
        GameEventManager.GetKeyingChange -= getkeying;
    }

    void Gameover() {
        game_status = 1;
    }
    void PlayerExit() {
        if(keynum == 2)game_status = 2;
    }

    bool getkey(){
        if(tryget){
            keynum++;
            tryget = false;
            return true;
        }
        return false;
    }
    void getkeying(bool trying){
        tryget = trying;
    }
}
