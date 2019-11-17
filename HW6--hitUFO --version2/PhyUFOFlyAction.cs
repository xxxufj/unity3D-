using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyUFOFlyAction : SSAction {
    private Vector3 angle;  //飞行角度
    float speed;            //飞行初速度

    private PhyUFOFlyAction() { }
    public static PhyUFOFlyAction GetSSAction(Vector3 angle, float speed) {
        //初始化物体将要运动的初速度向量
        PhyUFOFlyAction action = CreateInstance<PhyUFOFlyAction>();
        action.angle = angle;
        action.speed = speed;
        return action;
    }

    public override void Start() {
        //使用重力以及给一个初速度
        gameobject.GetComponent<Rigidbody>().velocity = angle * speed;
        gameobject.GetComponent<Rigidbody>().useGravity = true;
    }
}
