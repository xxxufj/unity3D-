using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gameModels;

public class ISceneController : MonoBehaviour, IUserAction {
    CharacterModel[] characters;
    CoastModel leftCoast, rightCoast;
    RiverModel river;
    BoatModel boat;
    bool winGame;
    bool loseGame;

    // Start is called before the first frame update
    void Start() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        loadResources();
    }

    // Update is called once per frame
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
        winGame = loseGame = false;
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
        Destroy(boat.getBoat().GetComponent<Move>());
        for (int i = 0; i < 6; i++) {
            Destroy( characters[i].getCharacter().GetComponent<ClickGUI>());
        }
    }

    public void moveBoat() {
        if (boat.isEmpty() || boat.isSailing()) {
            return;
        }
        checkGameState();
        boat.moveToAnotherCoast();
    }

    public void moveCharacter(CharacterModel character) {
        if (boat.isSailing()) return;
        if (character.isOnBoat()) {
            character.setCoastName(boat.getCoastName());
            if (character.getCoastName() == "leftCoast") {
                character.setPosOnCoast(leftCoast.getVacantIndex());
            }
            else character.setPosOnCoast(rightCoast.getVacantIndex());
            character.leaveBoat();
            boat.getOff(character.getSeatIndex());
            if (boat.isEmpty()) checkGameState();
        }
        else {    
            if (boat.isFull() || boat.getCoastName() != character.getCoastName()) return;
            character.setSeat(boat.getVacantIndex());
            if (character.getCoastName() == "leftCoast") {
                leftCoast.getOff(character.getPosIndexOnCoast());
            }
            else rightCoast.getOff(character.getPosIndexOnCoast());
            character.moveWithBoat(boat);
        }
    }

    public void checkGameState() {
        int countOfDevil_1, countOfDevil_2, countOfPriest_1, countOfPriest_2;
        countOfDevil_1 = countOfDevil_2 = countOfPriest_1 = countOfPriest_2 = 0;
        for (int i = 0; i < 6; i++) {
            if (characters[i].isOnBoat()) continue;
            if (characters[i].getCoastName() == "leftCoast") {
                if (characters[i].getType() == "priest") countOfPriest_1++;
                else countOfDevil_1++;
            }
            else {
                if (characters[i].getType() == "priest") countOfPriest_2++;
                else countOfDevil_2++;
            }
        }
        if (countOfDevil_1 > countOfPriest_1 && countOfPriest_1 > 0 || countOfDevil_2 > countOfPriest_2 && countOfPriest_2 > 0) {
            loseGame = true;
            stopGame();
        }
        else if (countOfPriest_1 + countOfDevil_1 >= 6) {
            winGame = true;
            stopGame();
        }
    }

    public bool win() {
        return winGame;
    }

    public bool lose() {
        return loseGame;
    }
}
