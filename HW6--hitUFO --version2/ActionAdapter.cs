using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionAdapter{
    void setRound(int round);
    void freeAction(DiskModel disk);
    void UFOFly(DiskModel disk);
}
