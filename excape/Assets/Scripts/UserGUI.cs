using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    private GUIStyle score_style = new GUIStyle();
    private GUIStyle text_style = new GUIStyle();
    private GUIStyle over_style = new GUIStyle();
    void Start () {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        text_style.normal.textColor = new Color(0, 0, 0, 1);
        text_style.fontSize = 16;
        score_style.normal.textColor = new Color(1,0.92f,0.016f,1);
        score_style.fontSize = 16;
        over_style.fontSize = 25;
    }

    private void OnGUI() {
        GUI.Label(new Rect(Screen.width / 2 - 80, 10, 100, 100), "WASD移动，方向键移动视角", text_style);
        GUI.Label(new Rect(Screen.width / 2 - 80, 30, 100, 100), "空格跳跃，Shift奔跑,按j拾取钥匙", text_style);
        GUI.Label(new Rect(Screen.width / 2 - 80, 50, 100, 100), "拾取所有钥匙，逃到出口胜利", text_style);
        int status = action.GetGamestatus();
        if (status ==1 ||status == 2) {//失败或胜利
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), status == 1?"游戏结束":"游戏胜利", over_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "重新开始")) {
                action.Restart();
                return;
            }
        }
    }
}
