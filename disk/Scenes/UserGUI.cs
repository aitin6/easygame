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
            GUI.Label(new Rect(Screen.width / 2 - 85, 10, 200, 50), "击败恶魔，保护小兔子");
            GUI.Label(new Rect(Screen.width / 2 - 150, 30, 350, 50), "子弹打中恶魔得分，让恶魔逃跑丢分，打中小兔子丢分");
            GUI.Label(new Rect(Screen.width / 2 - 85, 50, 250, 50), "点击屏幕进行射击");
        }
        //游戏准备
        if (sign == 0)
        {
            if (GUI.Button (new Rect (Screen.width / 2 - 80, Screen.height / 2, 160, 20), "开始游戏")){
                action.gamestart();
            }
        }
        //游戏中
        else if(sign == 1){
            string say = "当前回合回合数为" + action.getround().ToString() + 
            ",分数为" + ScoreController.GetScoreController().getscore().ToString() +"分";
            GUI.Box (new Rect (Screen.width / 2 - 100, 70, 200,30), say);
        }
        //游戏结束
        else if (sign == 2||sign == 3)
        {
            string say;
            if(sign == 2)say = "你输了,一共坚持了" + action.getround().ToString() + "局";
            else say = "你赢了,一共得了" + ScoreController.GetScoreController().getscore().ToString() +"分";
            GUI.Box (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 50, 200, 30), say);
            if (GUI.Button (new Rect (Screen.width / 2 - 80, Screen.height / 2, 160, 20), "重开")){
                action.gamestart();
                sign = 1;
            }
        }
    }
}
