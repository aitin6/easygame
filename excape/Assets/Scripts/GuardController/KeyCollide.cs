using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollide : MonoBehaviour
{    
    void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag == "Player"&&Singleton<GameEventManager>.Instance.PlayerGetkey()) {
            Destroy(this.gameObject);
        }
    }
}
