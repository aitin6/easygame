using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardActionManager : SSActionManager, ISSActionCallback {
    private GuardPatrolAction patrol;
    public void GuardPatrol(GameObject guard) {
        patrol = GuardPatrolAction.GetSSAction(guard.transform.position);
        this.RunAction(guard, patrol, this);
    }

    public void SSActionEvent(
        SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, GameObject objectParam = null) {
        if (intParam == 0) {
            //追逐
            GuardFollowAction follow = GuardFollowAction.GetSSAction();
            this.RunAction(objectParam, follow, this);
        } else {
            //巡逻
            GuardPatrolAction move = GuardPatrolAction.GetSSAction(objectParam.gameObject.GetComponent<GuardData>().start_position);
            this.RunAction(objectParam, move, this);
        }
    }
}
