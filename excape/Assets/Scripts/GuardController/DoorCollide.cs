using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollide : MonoBehaviour
{
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            Singleton<GameEventManager>.Instance.PlayerExit();
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            Singleton<GameEventManager>.Instance.PlayerExit();
        }
    }
}
