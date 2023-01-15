using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController {
    void LoadResources();
}

public interface IUserAction {
    //得到游戏状态
    int GetGamestatus();
    //重新开始
    void Restart();
}

public enum SSActionEventType : int { Started, Competeted }
public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, GameObject objectParam = null);
}

public interface IGameStatusOp {
    void PlayerEscape();
    void PlayerGameover();
}
