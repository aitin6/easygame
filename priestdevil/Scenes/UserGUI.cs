using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;
public class UserGUI : MonoBehaviour {

    private IUserAction action;
    public int sign = 0;

    bool isShow = false;
    void Start()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }
    void OnGUI()
    {
        //规则展示
        if (GUI.Button(new Rect(10, 10, 60, 30), "Rule", new GUIStyle("button")))
        {
            if (isShow)
                isShow = false;
            else
                isShow = true;
        }
        if(isShow)
        {
            GUI.Label(new Rect(Screen.width / 2 - 85, 10, 200, 50), "让全部牧师和恶魔都渡河");
            GUI.Label(new Rect(Screen.width / 2 - 120, 30, 250, 50), "每一边恶魔数量都不能多于牧师数量");
            GUI.Label(new Rect(Screen.width / 2 - 85, 50, 250, 50), "点击牧师、恶魔、船移动");
        }
        //游戏结束
        if (sign == 1||sign == 2)
        {
            string say;
            say = sign==1?"你输了":"你赢了";
            GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 100), say);
            if (GUI.Button (new Rect (Screen.width / 2 - 80, Screen.height / 2, 160, 20), "重开")){
                action.Restart();
                sign = 0;
            }
        }
    }
}
