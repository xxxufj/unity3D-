using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UserAction{
    void movePlayer(float h, float v);
    int getScore();
    int getGameStatus();
    float getHealth();
}
