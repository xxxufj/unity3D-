
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour{
    GameObject player;
    Vector3 velocity;
    int speed = 20;
    Vector3 lastPos;
    Animator animator;
    bool living = true;

    public void Start() {
        player = Object.Instantiate(Resources.Load<GameObject>("Player"), Vector3.zero, Quaternion.identity);
        player.transform.position = new Vector3(0, 2, 0);
        lastPos = new Vector3(0, 2, 0);
        animator = player.GetComponent<Animator>();
    }

     public void Update() {
        lastPos.y = 2;
        player.transform.position = lastPos;
        //默认动画
        if (living) animator.SetInteger("anistate", 0);
    }

    public void run(float h, float v) {
        if (h > 0) player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, 90, player.transform.rotation.z);
        if (h < 0) player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, -90, player.transform.rotation.z);
        if (v > 0) player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, 0, player.transform.rotation.z);
        if (v < 0) player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, 180, player.transform.rotation.z);
        velocity = new Vector3(h, 0, v);
        lastPos += velocity * speed * Time.fixedDeltaTime;
        if (h != 0 || v != 0) {
            //跑步动画
            animator.SetInteger("anistate", 1);
        }
    }

    public void dead() {
        living = false;
        //死亡动画
        animator.SetInteger("anistate", 2);
    }

    public GameObject getPlayer() {
        return player;
    }
}
