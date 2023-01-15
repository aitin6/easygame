using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {
    public delegate void ExitEvent();
    public static event ExitEvent ExitChange;
    
    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverChange;
    public delegate void GetKeyingEvent(bool trying);
    public static event GetKeyingEvent GetKeyingChange;
    public delegate bool GetKeyEvent();
    public static event GetKeyEvent GetKeyChange;

    public void PlayerExit() {
        if (ExitChange != null) {
            ExitChange();
        }
    }

    public void PlayerGameover(){
        if (GameoverChange != null) {
            GameoverChange();
        }
    }

    public void PlayerGetkeying(bool trying){
        if (GetKeyingChange != null) {
            GetKeyingChange(trying);
        }
    }
    public bool PlayerGetkey(){
        if (GetKeyChange != null) {
            return GetKeyChange();
        }
        return false;
    }
}
