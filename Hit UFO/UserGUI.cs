using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction action;

    //每个GUI的style
    GUIStyle bold_style = new GUIStyle();
    GUIStyle text_style = new GUIStyle();
    GUIStyle over_style = new GUIStyle();
    bool show;
    int round;
    bool changeRound;
    bool playing = true;

    void Start() {
        show = true;
        changeRound = false;
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    void OnGUI() {
        if (!playing) {
            if (action.getLife() < 0) {
                GUI.Button(new Rect(0, 0, Screen.width, Screen.width), "YOU LOSE");
            }
            else {
                GUI.Button(new Rect(0, 0, Screen.width, Screen.width), "YOU WIN");
            }
            return;
        }

        bold_style.normal.textColor = new Color(1, 0, 0);
        bold_style.fontSize = 16;
        text_style.normal.textColor = new Color(0, 0, 0, 1);
        text_style.fontSize = 16;
        over_style.normal.textColor = new Color(1, 0, 0);
        over_style.fontSize = 25;

        if (action.getLife() < 0) {
            playing = false;
        }

        if (changeRound) {
            GUI.Label(new Rect(Screen.width / 2 - 120, Screen.width / 2 - 220, 400, 100), " N E X T   R  O U N D ", over_style);
            if (GUI.Button(new Rect(0, 0, Screen.width, Screen.width), "press to continue")) {
                changeRound = false;
                action.startGame();
            }
        }
        else {
            if (show) {
                GUI.Label(new Rect(Screen.width / 2 - 170, Screen.width / 2 - 180, 400, 100), "大量UFO出现，点击它们即可消灭，快来加入战斗吧", text_style);
                if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.width / 2 - 120, 100, 50), "开始")) {
                    show = false;
                    action.startGame();
                }
            }
            else {
                GUI.Label(new Rect(Screen.width - 120, 20, 200, 50), "score:", text_style);
                GUI.Label(new Rect(Screen.width - 70, 20, 200, 50), action.getScore().ToString(), bold_style);

                GUI.Label(new Rect(Screen.width - 120, 50, 200, 50), "trial:", text_style);
                GUI.Label(new Rect(Screen.width - 70, 50, 200, 50), (action.getTrail() + 1).ToString(), bold_style);

                GUI.Label(new Rect(Screen.width - 120, 80, 50, 50), "life:", text_style);
                GUI.Label(new Rect(Screen.width - 70, 80, 50, 50), action.getLife().ToString(), bold_style);

                if (action.getRound() > round) {
                    round = action.getRound();
                    if (round > 2) playing = false;
                    changeRound = true;
                }
            }
        }
    }
}