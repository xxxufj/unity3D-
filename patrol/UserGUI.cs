using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private UserAction action;
    bool show = false;
    int treasure = 0;
    GUIStyle text_style = new GUIStyle();
    // Use this for initialization
    void Start() {
        action = SSDirector.getInstance().currentSceneController;
        text_style.normal.textColor = new Color(1, 0, 0);
        text_style.fontSize = 16;
    }

    // Update is called once per frame
    void Update() {
        if (action.getGameStatus() == 0) {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            action.movePlayer(h, v);
        }
    }

    private void OnGUI() {
        if (GUI.Button(new Rect(Screen.width - 610, 10, 150, 30), "操作方法")) {
            if (show) show = false;
            else show = true;
        }
        if (show) {
            GUI.Box(new Rect(Screen.width - 610, 42, 150, 90), "");
            GUI.Label(new Rect(Screen.width - 590, 45, 250, 30), "↑   键 ：     向前跑");
            GUI.Label(new Rect(Screen.width - 590, 65, 250, 30), "↓   键 ：     向后跑");
            GUI.Label(new Rect(Screen.width - 590, 85, 250, 30), "← 键 ：     向左跑");
            GUI.Label(new Rect(Screen.width - 590, 105, 250, 30), "→ 键 ：     向右跑");

        }
        GUI.Label(new Rect(Screen.width - 75, 20, 30, 50), "分数 ：", text_style);
        GUI.Label(new Rect(Screen.width - 25, 20, 200, 50), action.getScore().ToString(), text_style);
        GUI.Label(new Rect(Screen.width - 85, 50, 50, 50), "宝箱数 ：", text_style);
        GUI.Label(new Rect(Screen.width - 25, 50, 200, 50), treasure.ToString(), text_style);

        if (action.getGameStatus() == 1) {
            treasure = 1;
            GUI.Button(new Rect(0, 0, Screen.width, Screen.width), "YOU WIN");
        }
        else if (action.getGameStatus() == -1) {
            GUI.Button(new Rect(0, 0, Screen.width, Screen.width), "YOU LOSE");
        }
    }
}
