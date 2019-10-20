using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISceneController : MonoBehaviour, UserAction
{
    int status = 0;
    int score = 0;
    SSDirector director;
    PatrolFactory patrolFactory;
    PatrolManager actionManager;
    GameObject maze;
    GameObject treasure;
    UserGUI userGUI;
    public Player player;

    // treasure
    void Start()
    {
        maze = Object.Instantiate(Resources.Load<GameObject>("maze"), Vector3.zero, Quaternion.identity);
        treasure = Object.Instantiate(Resources.Load<GameObject>("treasure_chest"), Vector3.zero, Quaternion.identity);
        player = gameObject.AddComponent<Player>() as Player;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        director = SSDirector.getInstance();
        director.currentSceneController = this;
        patrolFactory = new PatrolFactory();
        actionManager = gameObject.AddComponent<PatrolManager>() as PatrolManager;
        for (int i = 0; i < 9; i++) {
            actionManager.PatrolAround(patrolFactory.getPatrol(i));
        }
        initTreasure();
    }

    // Update is called once per frame
    void Update()
    {
       if(status == 1) {
            treasure.SetActive(false);
        }
    }

    //初始化宝箱位置
    void initTreasure() {
        int index = 4;
        //宝箱不能够在地图中间
        while (index == 4) {
            index = Random.Range(0, 9);
        }
        treasure.transform.position = patrolFactory.getPos(index);
    }

    public void movePlayer(float h, float v) {
        player.run(h, v);
    }

    void OnEnable() {
        Publisher.startAttack += attackPlayer;
        Publisher.winGame += winGame;
    }

    void OnDisable() {
        Publisher.startAttack -= attackPlayer;
        Publisher.winGame -= winGame;
    }

    public int getScore() {
        return score;
    }

    public void attackPlayer(int index) {
        actionManager.attackPlayer(index);
    }

    public void addScore() {
        score++;
    }

    public int getGameStatus() {
        return status;
    }

    public void winGame() {
        status = 1;
        actionManager.freeAction();
    }

    public void loseGame() {
        status = -1;
        player.dead();
        actionManager.freeAction();
    }
}
