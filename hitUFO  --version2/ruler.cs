using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//为飞碟设置合适的性能参数
public class Ruler {
    private int round;
    public void setRound(int round) {
        this.round = round;
    }

    //获得飞碟出现的位置
    public Vector3 getStart() {
        int x = Random.Range(-25, 25);  //相机能够看到的位置
        int y = Random.Range(-25, 25);  //相机能够看到的位置
        int z = Random.Range(-5, 5);    //将位置局限在（-5.5），以免由于 z 距离过远影响游戏体验
        return new Vector3(x, y, z);
    }

    public Vector3 getAngle() {
        int xFlag = Random.Range(0, 2);
        int yFlag = Random.Range(0, 2);
        float x = Random.Range(0, 0.50f);//angle_x属于（0，0.5）
        float y = 1 - x;                 //angle_y = 1-x
        float z = 0;    //将z设为0使飞碟的运动轨迹始终保持在x-y平面上，有利于游戏体验
        if (xFlag == 1) x *= -1;    //随机将角度设为负数
        if (xFlag == 1) y *= -1;
        return new Vector3(x, y, z);
    }

    //设置速度
    public float getSpeed() {
        //飞碟速度随着round增加而增加
        return 10 + round * 3;
    }
}