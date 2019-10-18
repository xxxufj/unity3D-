using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour, ISSActionCallback{
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback callback) {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = callback;
        action.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed) {

    }
}

public class FlyActionManager : SSActionManager, ActionAdapter {
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
        disk.disk.GetComponent<Rigidbody>().useGravity = false;
        disk.disk.GetComponent<Rigidbody>().velocity = Vector3.zero;
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

