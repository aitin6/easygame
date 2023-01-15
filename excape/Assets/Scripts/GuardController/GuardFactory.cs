using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFactory : MonoBehaviour {
    private GameObject guard = null;                               //巡逻兵
    private List<GameObject> used = new List<GameObject>();        //正在使用的巡逻兵列表
    private Vector3[] vec = new Vector3[4];                        //每个巡逻兵的初始位置

    public List<GameObject> GetPatrols() {
        int[] pos_x = { 6, 0,6, 0};
        int[] pos_z = { 15, 9,-21 ,-27 };
        for(int i=0;i < 4;i++) {
                vec[i] = new Vector3(pos_x[i], 0, pos_z[i]);
        }
        for(int i = 0; i < 4; i++) {
            guard = Instantiate(Resources.Load<GameObject>("Prefabs/Guard"));
            guard.transform.position = vec[i];
            guard.GetComponent<GuardData>().sign = i/2;
            guard.GetComponent<GuardData>().start_position = vec[i];
            guard.GetComponent<Animator>().SetFloat("forward", 1);
            used.Add(guard);
        }   
        return used;
    }
}
