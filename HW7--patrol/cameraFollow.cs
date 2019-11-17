using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

    private Transform target;//目标         
    private Vector3 offset;//目标与相机的距离  
    
    void Start() {
        offset = new Vector3(0, 30, -20);
    }
    void Update() {
        target = SSDirector.getInstance().currentSceneController.player.getPlayer().transform;
        transform.position = target.position + offset;
    }
}

