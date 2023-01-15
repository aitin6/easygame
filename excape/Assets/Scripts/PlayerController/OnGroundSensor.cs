﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour {
    public CapsuleCollider capcol;
    //碰撞体往下偏移
    public float offset = 0.1f;
    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    void Awake() {
        radius = capcol.radius;
    }

    void FixedUpdate() {
        //死亡与拾取时停止运动

        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius,LayerMask.GetMask("Ground"));        
        if (outputCols.Length != 0) {
            SendMessageUpwards("OnGround");
        } else {
            SendMessageUpwards("NotOnGround");
        }
    }
}
