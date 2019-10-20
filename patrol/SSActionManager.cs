using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour{
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback callback) {
        action.transform = gameobject.transform;
        action.target = gameobject.transform.position;
        action.initPos = gameobject.transform.position;
        action.animator = gameobject.GetComponent<Animator>();
        action.callback = callback;
        action.Start();
    }
}