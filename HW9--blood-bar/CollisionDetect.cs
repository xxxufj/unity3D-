using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    Publisher publisher;
    void Start() {
        publisher = gameObject.AddComponent<Publisher>() as Publisher;
    }

    void OnTriggerEnter(Collider collider) {  
        if (collider.gameObject.tag == "Patrol") {
            int index = collider.gameObject.name[0] - '0';
            publisher.NotifyAttack(index);
        }
        else if(collider.gameObject.tag == "treasure") {
            publisher.notifyWin();
        }
    }
}
