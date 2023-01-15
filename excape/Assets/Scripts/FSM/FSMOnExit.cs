using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMOnExit : StateMachineBehaviour {
    public string[] onExitMessages;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (var msg in onExitMessages) {
            Debug.Log(animator.gameObject);
            animator.gameObject.SendMessageUpwards(msg);
        }
    }
}
