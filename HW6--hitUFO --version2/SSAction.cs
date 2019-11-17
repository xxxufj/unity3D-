using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Completed }

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed);
}

public class SSAction : ScriptableObject {
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameobject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}

//调整设置
public class FlyAction : SSAction{                           
    private Vector3 angle;
    private float speed;

    //根据飞行角度和速度获取一个FlyAction
    public static FlyAction GetSSAction(Vector3 angle, float speed) {
        FlyAction action = CreateInstance<FlyAction>();
        action.angle = angle;
        action.speed = speed;
        return action;
    }

    //实现简单的直线飞行
    public override void Update() {
        transform.position += angle * Time.deltaTime * speed;
    }

    public override void Start() {
        Update();
    }
}

