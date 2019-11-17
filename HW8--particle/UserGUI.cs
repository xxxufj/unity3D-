using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    public flare flare1, flare2, flare3;
    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
       
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 30, 50, 30), "东风")) {
            flare1.increaseWindPower();
            flare2.increaseWindPower();
            flare3.increaseWindPower();
        }

        if (GUI.Button(new Rect(10, 70, 50, 30), "西风")) {
            flare1.decreaseWindPower();
            flare2.decreaseWindPower();
            flare3.decreaseWindPower();
        }

        if (GUI.Button(new Rect(10, 110, 50, 30), "增强")) {
            flare1.increaseFlame();
            flare2.increaseFlame();
            flare3.increaseFlame();
        }
        
        if (GUI.Button(new Rect(10, 150, 50, 30), "减弱")) {
            flare1.decreaseFlame();
            flare2.decreaseFlame();
            flare3.decreaseFlame();
        }
    }
}
