using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject {
    public Vector3 target;          //目标
    public Animator animator;       //动画
    public Vector3 initPos;         //动作的初始位置

    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }

    public float Abs(float x) {
        return x > 0 ? x : -x;
    }
}
