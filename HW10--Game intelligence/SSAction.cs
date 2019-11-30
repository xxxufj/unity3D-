using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gameModels;

public enum SSActionEventType : int { Started, Completed }

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed);
}

public class SSAction : ScriptableObject {
    public bool enable = true;
    public bool destroy = false;

    public GameObject gameobject { get; set; }
    public Transform transform { get; set; }
    public ISSActionCallback callback { get; set; }

    protected SSAction() { }

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}

public class CCMoveToAction : SSAction {
    public Vector3 target;
    public float speed;

    public static CCMoveToAction GetSSAction(Vector3 target, float speed) {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update() {
        if (enable) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed);
            if (this.transform.position == target) {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public override void Start() {
        Update();
    }
}

public class SequenceAction : SSAction, ISSActionCallback {
    public List<SSAction> sequence;    
    public int repeat = -1;           
    public int start = 0;              

    public static SequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence) {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update() {
        if (sequence.Count == 0) return;
        if (start < sequence.Count) {
            sequence[start].Update();     
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed) {
        source.destroy = false;          
        this.start++;
        if (this.start >= sequence.Count) {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0) {
                this.destroy = true;               
                this.callback.SSActionEvent(this); 
            }
        }
    }

    public override void Start() {
        foreach (SSAction action in sequence) {
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;                
            action.Start();
        }
    }

    void OnDestroy() {
        
    }
}

public class SSActionManager : MonoBehaviour, ISSActionCallback {
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback callback) {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = callback;
        action.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed) {

    }
}

public class CCActionManager : SSActionManager {
    private CCMoveToAction moveBoat;
    private SequenceAction moveCharacter;

    public ISceneController controller;

    public void Start() {
        controller = SSDirector.getInstance().currentSceneController;
        controller.actionManager = this;
    }

    public void Update() {
        if (moveBoat) moveBoat.Update();
        if (moveCharacter) moveCharacter.Update();
    }

    public void MoveBoat(GameObject boat, Vector3 target, float speed) {
        moveBoat = CCMoveToAction.GetSSAction(target, speed);
        RunAction(boat, moveBoat, this);
    }

    public void MoveCharacter(GameObject character, Vector3 target, float speed) {
        Vector3 forward, up, down;
        float jumpDistance = target.x < 0 ? -8 : 8;
       
        List<SSAction> sequence;
        if (character.transform.position.y > target.y) {
            forward = target;
            forward.x += jumpDistance;
            forward.y = character.transform.position.y;
            down = target;
            CCMoveToAction moveForward = CCMoveToAction.GetSSAction(forward, speed);
            CCMoveToAction jump = CCMoveToAction.GetSSAction(down, speed);
            sequence = new List<SSAction> { moveForward, jump };
        }
        else {
            forward = target;
            up = character.transform.position;
            up.x += jumpDistance;
            up.y = target.y;
            CCMoveToAction moveForward = CCMoveToAction.GetSSAction(forward, speed);
            CCMoveToAction jump = CCMoveToAction.GetSSAction(up, speed);
            sequence = new List<SSAction> { jump, moveForward };
        }

        moveCharacter = SequenceAction.GetSSAcition(1, 0, sequence);
        RunAction(character, moveCharacter, this);
    }
}