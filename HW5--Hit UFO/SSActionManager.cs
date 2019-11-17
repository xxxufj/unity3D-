using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour, ISSActionCallback {
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback callback) {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = callback;
        action.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed) {

    }
}

public class FlyActionManager : SSActionManager {
    public FlyAction[] actions = new FlyAction[20]; //最多建立20个action
    public int[] diskID = new int[20];              //action 所应用的飞碟的ID
    public ISceneController controller;            
    private Ruler ruler;    //根据不同 round 对飞碟的性能参数做设置
    int round;

    protected void Start() {
        controller = (ISceneController)SSDirector.getInstance().currentSceneController;
        controller.actionManager = this;
        ruler = new Ruler();
        for(int i = 0; i < 20; i++) {
            diskID[i] = -1; //开始时没有可用飞碟，ID 均为-1
        }
    }

    public void Update() {
        for(int i = 0; i < 20; i++) {
            if (diskID[i] != -1) {
                //执行有附着在飞碟上的 action
                actions[i].Update();
            }
        }
    }

    public void setRound(int round) {
        this.round = round;
    }

    public void UFOFly(DiskModel disk) {
        ruler.setRound(round);
        disk.disk.transform.position = ruler.getStart();//设置飞碟的出现位置
        int index = 0;
        for (; diskID[index] != -1; index++) ;//找到空闲的 Action
        actions[index] = FlyAction.GetSSAction(ruler.getAngle(), ruler.getSpeed());
        diskID[index] = disk.getDiskID();
        this.RunAction(disk.disk, actions[index], this);
    }

    public void freeAction(DiskModel disk) {
        for(int i = 0; i < 20; i++) {
            //当飞碟不再需要时，actionManager可以简单的将对应的action设为空闲，而非直接删除 action
            if(diskID[i] == disk.getDiskID()) {
                diskID[i] = -1;
                break;
            }
        }
    }
}

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
        return 5 + round * 3;
    }
}