using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//跑步动作实现
public class PatrolRunAction : SSAction {
    const int speed = 15;
    public static PatrolRunAction GetRunAction() {
        PatrolRunAction run = CreateInstance<PatrolRunAction>();
        //追踪对象目标是玩家
        run.target = SSDirector.getInstance().currentSceneController.player.getPlayer().transform.position;
        return run;
    }

    public override void Start() {
       
    }

    public override void Update() {
        if (transform == null || target == null) return;

        //设定巡逻兵的 animation 为跑
        animator.SetInteger("anistate", 2);
        target = SSDirector.getInstance().currentSceneController.player.getPlayer().transform.position;

        //设定方向为追踪目标
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);

        //当追踪玩家时检测到距离小于设定值，则通知 manager 已经追踪到目标
        if (Abs(this.transform.position.x - target.x) < 2 && Abs(this.transform.position.z - target.z) < 2) {
            this.callback.SSActionEvent(this, SSActionEventType.Completed, 1);
            animator.SetInteger("anistate", 3);
        }
    }
}


public class PatrolWalkAction : SSAction {
    private PatrolState state;  //巡逻状态（位置）
    const int speed = 10;

    public static PatrolWalkAction GetWalkAction() {
        PatrolWalkAction walk = CreateInstance<PatrolWalkAction>();
        walk.state = PatrolState.forward;
        return walk;
    }

    public override void Start() {

    }

    public override void Update() {
        transform.LookAt(target);
        if (transform == null || target == null) return;

        //设定 animation 为走
        animator.SetInteger("anistate", 1);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);

        //当巡逻时发现接近目标，通知 manager，manager 更新下一个巡逻地点
        if (Abs(this.transform.position.x - target.x) < 0.01 && Abs(this.transform.position.z - target.z) < 0.01) {
            this.callback.SSActionEvent(this, SSActionEventType.Completed, 0);
        }
    }

    public PatrolState GetPatrolState() {
        return state;
    }

    public void SetPatrolState(PatrolState newState) {
        state = newState;
    }
}

