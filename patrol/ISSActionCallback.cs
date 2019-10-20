using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Completed }

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int type = 0);
}