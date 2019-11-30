using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gameModels;

public class UserGUI : MonoBehaviour {
    private UserGUI action;
    GUIStyle textStyle;
    void Start() {

    }
    void Update() {

    }
    void OnGUI() {
        textStyle = new GUIStyle() {
            fontSize = 30,
        };

        if (GUI.Button(new Rect(30,30,50,40),"restart")){
            SSDirector.getInstance().currentSceneController.restart();
        }

        if (GUI.Button(new Rect(30, 80, 50, 40), "tips")) {
            SSDirector.getInstance().currentSceneController.useTips();
        }

        if (SSDirector.getInstance().currentSceneController.getGameStatus() == GameStatus.win) {
            GUI.Label(new Rect(250, 100, 100, 50), "YOU WIN !!!", textStyle);
        }
        else if (SSDirector.getInstance().currentSceneController.getGameStatus() == GameStatus.lose) {
            GUI.Label(new Rect(250, 100, 100, 50), "Game Over~", textStyle);
        }
    }
}


