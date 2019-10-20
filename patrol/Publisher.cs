using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher : MonoBehaviour {

    public delegate void attack(int index);
    public static event attack startAttack;

    public delegate void win();
    public static event win winGame;

    public void NotifyAttack(int index) {
        if (startAttack != null) {
            startAttack(index);
        }
    }

    public void notifyWin() {
        if (winGame != null) {
            winGame();
        }
    }
}
