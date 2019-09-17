using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameModels {

    public class SSDirector : System.Object {
        private static SSDirector _instance;
        public ISceneController currentSceneController { get; set; }
        public static SSDirector getInstance() {
            if (_instance == null) {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }

    public interface IUserAction {
        void moveBoat();
        void moveCharacter(CharacterModel character);
        void restart();
    }

    public class CharacterModel {
        GameObject character;
        string type;
        string coastName;
        bool onBoat;
        int seat;
        int posIndexOnCoast;
        Vector3[] coastPositions = new Vector3[6];
        Vector3 leftSeat = new Vector3(-7.50f, 5, 0);
        Vector3 rightSeat = new Vector3(4.50f, 5, 0);
        ClickGUI clickScript;

        public CharacterModel(string type) {     
            this.type = type;
            character = Object.Instantiate(Resources.Load(type, typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            for (int i = 0; i < 6; i++) {
                coastPositions[i].Set(29 + i * 5, 5, 0);
            }
            initSettings();
        }

        public void initSettings() {
            if (character.GetComponent< ClickGUI>() == null) {
                clickScript = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
                clickScript.setCharacter(this);
            }
            onBoat = false;
            seat = -1;
            coastName = "rightCoast";
            character.transform.parent = null;
            clickScript.enabled = true;
        }

        public void setPosition(Vector3 newPos) {
            character.transform.position = newPos;
        }

        public GameObject getCharacter() {
            return character;
        }

        public string getType() {
            return type;
        }

        public int getSeatIndex() {
            return seat;
        }

        public void setSeat(int seat) {
            this.seat = seat;
        }

        public void setCoastName(string name) {
            coastName = name;
        }

        public string getCoastName() {
            return coastName;
        }

        public int getPosIndexOnCoast() {
            return posIndexOnCoast;
        }

        public void setPosOnCoast(int index) {
            this.posIndexOnCoast = index;
            Vector3 position = coastPositions[index];
            if (coastName == "leftCoast") position.x *= -1;
            setPosition(position);
        }

        public bool isOnBoat() {
            return onBoat;
        }

        public void moveWithBoat(BoatModel boat) {
            onBoat = true;
            Vector3 change = seat == 0 ? leftSeat : rightSeat;
            character.transform.position = boat.getBoat().transform.position + change;
            character.transform.parent = boat.getBoat().transform;
        }

        public void leaveBoat() {
            onBoat = false;
            character.transform.parent = null;
        }
    }


    /*--------------------river-------------------------*/
    public class RiverModel {
        GameObject river;

        public RiverModel() {
            river = Object.Instantiate(Resources.Load("river", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        }
    }

    public class CoastModel {
        GameObject coast;
        string coastName;
        bool[] isOccupied = new bool[6];

        public CoastModel(string coastName) {
            this.coastName = coastName;
            coast = Object.Instantiate(Resources.Load("coast", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            //位置调整
            if (coastName == "leftCoast") {
                coast.transform.position = new Vector3(-50, 1, 0);
            }
            else {
                coast.transform.position = new Vector3(50, 1, 0);
            }
            initSettings();
        }
           
        public void initSettings() {
            for (int i = 0; i < 6; i++) {
                isOccupied[i] = false;
            }
        }

        public int getVacantIndex() {
 
            for (int i = 0; i < 6; i++) {
                if (!isOccupied[i]) {
                    isOccupied[i] = true;
                    return i;
                }
            }
            return -1;
        }

        public void getOff(int posIndex) {
            isOccupied[posIndex] = false;
        }
    }


    public class BoatModel {
        GameObject boat;
        string coastName;
        bool leftOccupied, rightOccupied;
        Vector3 leftPort, rightPort;
        Move moveScript;
        ClickGUI clickScript;

        public BoatModel() {
            boat = Object.Instantiate(Resources.Load("boat", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            leftPort = new Vector3(-10, 1.50f, 0);
            rightPort = new Vector3(12, 1.50f, 0);
            initSettings();
        }

        public void initSettings() {
            if (boat.GetComponent<ClickGUI>() == null) {
                clickScript = boat.AddComponent(typeof(ClickGUI)) as ClickGUI;
                clickScript.setBoat(this);
            }
            if (boat.GetComponent<Move>() == null) {
                moveScript = boat.AddComponent(typeof(Move)) as Move;
            }
            boat.transform.position = rightPort;
            coastName = "rightCoast";
            moveScript.setMoveable(rightPort);
            leftOccupied = rightOccupied = false;
        }

        public GameObject getBoat() {
            return boat;
        }

        public bool isEmpty() {
            return leftOccupied == false && rightOccupied == false;
        }

        public bool isFull() {
            return leftOccupied == true && rightOccupied == true;
        }

        public bool isSailing() {
            return boat.transform.position != leftPort && boat.transform.position != rightPort;
        }

        public string getCoastName() {
            return coastName;
        }

        public int getVacantIndex() {
            if (!leftOccupied) {
                leftOccupied = true;
                return 0;
            }
            else {
                rightOccupied = true;
                return 1;
            }
        }

        public void getOff(int seat) {
            if (seat == 0) leftOccupied = false;
            else rightOccupied = false;
        }

        public void moveToAnotherCoast() {
            if (coastName == "leftCoast") {
                moveScript.setMoveable(rightPort);
                coastName = "rightCoast";
            }
            else {
                moveScript.setMoveable(leftPort);
                coastName = "leftCoast";
            }
        }
    }

    //moveable需要更改
    public class Move : MonoBehaviour {
        bool moveable;
        float speed;
        Vector3 dest = new Vector3(0, 0, 0);

        void Start() {
            moveable = false;
            speed = 10;
        }

        void Update() {
            if (moveable == true) {
                transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            }
        }

        public void setMoveable(Vector3 dest) {
            this.dest = dest;
            moveable = true;
        }
    }

    public class ClickGUI : MonoBehaviour {
        BoatModel boat = null;
        CharacterModel character = null;
        IUserAction respond;

        public void Start() {
            respond = SSDirector.getInstance().currentSceneController as IUserAction;
        }
        public void setBoat(BoatModel boat) {
            this.boat = boat;
        }

        public void setCharacter(CharacterModel character) {
            this.character = character;
        }

        void OnMouseDown() {
            if (boat != null) {
                SSDirector.getInstance().currentSceneController.moveBoat();
            }
            else {
                SSDirector.getInstance().currentSceneController.moveCharacter(character);
            }
        }
    }

}