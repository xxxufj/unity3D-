using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolState : int { forward, left, right, backward };

public class PatrolManager : SSActionManager, ISSActionCallback {
    PatrolWalkAction[] walk = new PatrolWalkAction[9];
    PatrolRunAction[] run = new PatrolRunAction[9];
    Patrol[] patrols = new Patrol[9];
    int[] state = new int[9];   //-1为空，0为巡逻，1为追击玩家

    public void Start() {
        //for (int i = 0; i < 9; i++) state[i] = -1;
    }

    public void Update() {
        for (int i = 0; i < 9; i++) {
            if (state[i] == 0) {
                    walk[i].Update();
            }
            else if(state[i] == 1){
                run[i].Update();
                //判断巡逻兵是否即将追出巡逻范围
                if (patrols[i].OutOfBorder()) {
                    //成功摆脱巡逻兵，分数加1
                    SSDirector.getInstance().currentSceneController.addScore();
                    //巡逻兵追出范围，需要回到初始位置
                    returnPatrol(patrols[i]);
                }
            }
        }
    }

    //巡逻兵 patrol 开始巡逻
    public void PatrolAround(Patrol patrol) {
        patrols[patrol.getIndex()] = patrol;
        //默认以woal状态开始巡逻
        walk[patrol.getIndex()] = PatrolWalkAction.GetWalkAction();
        //状态设置为巡逻
        state[patrol.getIndex()] = 0;
        Debug.Log("start patrol");
        this.RunAction(patrol.getGameObject(), walk[patrol.getIndex()], this);
    }

    public void attackPlayer(int index) {
        //状态设置为追踪
        state[index] = 1;
        run[index] = PatrolRunAction.GetRunAction();
        //以跑步状态进行追踪
        this.RunAction(patrols[index].getGameObject(), run[index], this);
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int type = 0) {
        //巡逻时触发，设置目标为下一个地点
        if (type == 0) {
            PatrolWalkAction walk = (PatrolWalkAction)source;
            switch (walk.GetPatrolState()) {
                case PatrolState.forward:
                    TurnLeft(walk);
                    break;
                case PatrolState.left:
                    TurnBackward(walk);
                    break;
                case PatrolState.backward:
                    TurnRight(walk);
                    break;
                case PatrolState.right:
                    TurnForward(walk);
                    break;
            }
        }
        //追踪玩家时触发，玩家死亡，游戏结束
        if(type == 1) {
            SSDirector.getInstance().currentSceneController.reduceBlood();
        }
    }

    //当失去目标时，巡逻兵回到其巡逻的初始位置
    public void returnPatrol(Patrol patrol) {
        state[patrol.getIndex()] = 0;
        walk[patrol.getIndex()].SetPatrolState(PatrolState.forward);
        walk[patrol.getIndex()].target = patrol.getInit();
    }

    //向左巡逻
    public void TurnLeft(PatrolWalkAction source) {
        source.SetPatrolState(PatrolState.left);
        source.target += Vector3.left * Random.Range(15, 25);
    }

    //向右巡逻
    public void TurnRight(PatrolWalkAction source) {
        source.SetPatrolState(PatrolState.right);
        source.target += Vector3.right * Random.Range(15, 25);
    }

    //向前巡逻
    public void TurnForward(PatrolWalkAction source) {
        source.SetPatrolState(PatrolState.forward);
        source.target = source.initPos;
    }

    //向后巡逻
    public void TurnBackward(PatrolWalkAction source) {
        source.SetPatrolState(PatrolState.backward);
        source.target += Vector3.back * Random.Range(15, 25);
    }

    //释放动作
    public void freeAction() {
        for(int i = 0; i < 9; i++) {
            walk[i].target = walk[i].transform.position;
            if (run[i] && run[i].transform != null) run[i].target = run[i].transform.position;
            state[i] = -1;
        }
    }
}
