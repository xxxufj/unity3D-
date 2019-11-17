using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyFlyActionManager : SSActionManager, ActionAdapter {

    public PhyUFOFlyAction[] actions = new PhyUFOFlyAction[20]; //最多建立20个action
    public int[] diskID = new int[20];              //action 所应用的飞碟的ID
    public ISceneController controller;
    private Ruler ruler;                            //根据不同 round 对飞碟的性能参数做设置
    int round;

    protected void Start() {
        controller = (ISceneController)SSDirector.getInstance().currentSceneController;
        controller.actionManager = this;
        ruler = new Ruler();
        for (int i = 0; i < 20; i++) {
            diskID[i] = -1; //开始时没有可用飞碟，ID 均为-1
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

        Vector3 angle = Vector3.left;
        int flag = Random.Range(0, 2);
        if (flag == 1) angle *= -1;

        actions[index] = PhyUFOFlyAction.GetSSAction(angle, ruler.getSpeed());//从ruler中获取初速度和飞行角度，加速度为10
        diskID[index] = disk.getDiskID();
        this.RunAction(disk.disk, actions[index], this);
    }

    public void freeAction(DiskModel disk) {
        disk.disk.GetComponent<Rigidbody>().velocity = Vector3.zero;
        disk.disk.GetComponent<Rigidbody>().useGravity = false;

        for (int i = 0; i < 20; i++) {
            //当飞碟不再需要时，actionManager可以简单的将对应的action设为空闲，而非直接删除 action
            if (diskID[i] == disk.getDiskID()) {
                diskID[i] = -1;
                break;
            }
        }
    }
}

