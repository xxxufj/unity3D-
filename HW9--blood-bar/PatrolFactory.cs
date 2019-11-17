using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory : MonoBehaviour {
    //九个巡逻兵的初始位置
    private int[,] initPos = new int[,] {
        { 50, 2, 50},
        { 10, 2, 50 },
        { -30, 2, 50 },
        { 50, 2, 10  },
        { 10, 2, 10 },
        { -30, 2, 10 },
        { 50, 2, -30  },
        { 10, 2, -30  },
        { -30, 2, -30 },
    };

    //生产一个新的巡逻兵
    public Patrol getPatrol(int index) {
        Vector3 pos = new Vector3(initPos[index, 0], initPos[index, 1], initPos[index, 2]);
        Patrol patrol = new Patrol(index, pos);
        return patrol;
    }

    //根据地图块编号获取其中央的位置
    public Vector3 getPos(int index) {
        Vector3 ret;
        ret.x = initPos[index, 0] - 10;
        ret.y = 1;
        ret.z = initPos[index, 2] - 10;
        return ret;
    }
}
