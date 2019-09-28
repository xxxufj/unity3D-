using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gameModels;

public class ISceneController : MonoBehaviour, IUserAction {
    CharacterModel[] characters;
    CoastModel leftCoast, rightCoast;
    RiverModel river;
    BoatModel boat;
    public CCActionManager actionManager;
    public Judge judge;

    public GameStatus status;

    void Start() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        loadResources();
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
        judge = gameObject.AddComponent<Judge>() as Judge;
        judge.setJudge(characters, boat, this);
        status = GameStatus.playing;
    }

    void Update() {

    }

    private void loadResources() {
        river = new RiverModel();
        boat = new BoatModel();
        leftCoast = new CoastModel("leftCoast");
        rightCoast = new CoastModel("rightCoast");
        characters = new CharacterModel[6];

        for (int i = 0; i < 3; i++) {
            characters[i] = new CharacterModel("priest");
            characters[i].setPosOnCoast(rightCoast.getVacantIndex());
        }
        for (int i = 3; i < 6; i++) {
            characters[i] = new CharacterModel("devil");
            characters[i].setPosOnCoast(rightCoast.getVacantIndex());
        }
    }

    public void restart() {
        initSettings();
    }

    private void initSettings() {
        status = GameStatus.playing;
        leftCoast.initSettings();
        rightCoast.initSettings();
        for (int i = 0; i < 6; i++) {
            characters[i].initSettings();
            characters[i].setPosOnCoast(rightCoast.getVacantIndex());
        }
        boat.initSettings();
    }

    private void stopGame() {
        Destroy(boat.getBoat().GetComponent<ClickGUI>());
        for (int i = 0; i < 6; i++) {
            Destroy( characters[i].getCharacter().GetComponent<ClickGUI>());
        }
    }

    public void moveBoat() {
        if (boat.isEmpty() || boat.isSailing()) return;

        //通过 actionManager 间接控制动作发生
        actionManager.MoveBoat(boat.getBoat(), boat.getAndSetAnotherPort(), 0.50f);
        for (int i = 0; i < 6; i++) {
            if (characters[i].isOnBoat()) {
                characters[i].setCoastName(boat.getCoastName());
            }
        }
    }

    public void moveCharacter(CharacterModel character) {
        if (boat.isSailing()) return;
        if (character.isOnBoat()) {
            //character.setCoastName(boat.getCoastName());
            character.leaveBoat();
            boat.getOff(character.getSeatIndex());

            Vector3 newPos;
            int index;
            if (character.getCoastName() == "leftCoast") {
                index = leftCoast.getVacantIndex();
                newPos = character.getCoastPosition(index);
                newPos.x = -newPos.x;
            }
            else {
                index = rightCoast.getVacantIndex();
                newPos = character.getCoastPosition(index);
            }
            character.setPosIndexOnCoast(index);
            actionManager.MoveCharacter(character.getCharacter(), newPos, 0.50f);
        }
        else {
            if (boat.isFull() || boat.getCoastName() != character.getCoastName()) return;
            int index = character.getPosIndexOnCoast();       
            if (character.getCoastName() == "leftCoast") leftCoast.getOff(character.getPosIndexOnCoast());
            else rightCoast.getOff(character.getPosIndexOnCoast());
            int seat = boat.getVacantIndex();
            character.setSeat(seat);

            actionManager.MoveCharacter(character.getCharacter(),boat.getBoat().transform.position + 
                character.getBoatPosition(seat), 0.50f);
            character.board(boat.getBoat().transform);
        }
    }

    public GameStatus getGameStatus() {
        return status;
    }
}

public enum GameStatus : int { win, lose, playing }

public interface ISSJudgeCallback {
    void SSJudgeEvent(ISceneController source, GameStatus status = GameStatus.playing);
}

public class Judge :MonoBehaviour, ISSJudgeCallback {
    CharacterModel[] roles = new CharacterModel[6];
    ISceneController callback;
    BoatModel boat;

    int countOfDevil_1, countOfDevil_2, countOfPriest_1, countOfPriest_2;

    public void setJudge(CharacterModel[] characters, BoatModel boat, ISceneController source) {
        for(int i = 0; i < 6; i++) {
            this.roles[i] = characters[i];
        }
        this.boat = boat;
        callback = source;
    }

    public void SSJudgeEvent(ISceneController source, GameStatus status = GameStatus.playing) {
        source.status = status;
    }

    public void Update() {
        if (!boat.isSailing()) return;  //在船行驶的时候才对游戏状态进行判断
        if (callback.getGameStatus() == GameStatus.playing) {
            countOfDevil_1 = countOfDevil_2 = countOfPriest_1 = countOfPriest_2 = 0;
            for (int i = 0; i < 6; i++) {
                if (roles[i].getCoastName() == "leftCoast") {
                    if (roles[i].getType() == "priest") countOfPriest_1++;
                    else countOfDevil_1++;
                }
                else {
                    if (roles[i].getType() == "priest") countOfPriest_2++;
                    else countOfDevil_2++;
                }
            }
            if (countOfDevil_1 > countOfPriest_1 && countOfPriest_1 > 0 || countOfDevil_2 > countOfPriest_2 && countOfPriest_2 > 0) {
                SSJudgeEvent(callback, GameStatus.lose);
            }
            else if (countOfPriest_1 + countOfDevil_1 >= 6) {
                SSJudgeEvent(callback, GameStatus.win);
            }
        }
    }


}