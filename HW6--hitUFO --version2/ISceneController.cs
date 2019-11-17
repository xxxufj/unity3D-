using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

//IUserAction有获取分数、血量、trail、round，以及启动游戏的功能
public interface IUserAction {
    int getScore();
    int getLife();
    int getTrail();
    int getRound();
    void startGame();
}


public class ISceneController : MonoBehaviour, IUserAction {
    public ActionAdapter actionManager;
    public DiskFactory diskFactory;

    private List<DiskModel> currentDisks = new List<DiskModel>();
    private int diskCount = 0;  //当前场景中的飞碟数量
    private int[] maxCount = { 3, 5, 8 };   //每个round中飞碟需要维持的数目，数目不足时将发送新的飞碟
    private int round = 0;  //当前游戏的 round
    private int currentTrial = 0;//当前游戏的 trail
    private int score = 0;  //获得的分数
    private int[] scoreOfRound = { 10, 20, 30 };   //每个round需要达成的分数目标
    private bool playing = false;
    private int life = 100;  //血量

    public UserGUI userGUI;

    void Start() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        diskFactory = Singleton<DiskFactory>.Instance;
        actionManager = gameObject.AddComponent<PhyFlyActionManager>() as ActionAdapter;
        actionManager.setRound(round);
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
    }

    void Update() {
        //发送飞碟
        if (playing) {
            if (diskCount < maxCount[round]) {
                sendDisk();
            }
            //检查当前游戏状态是否允许升级
            checkStatus();
            removeRemoteDisk();

            //检查玩家的射击操作
            if (Input.GetButtonDown("Fire1")) {
                Vector3 mp = Input.mousePosition; 
                Hit(mp);
            }
        }
    }

    public void startGame() {
        life = 100;
        playing = true;
    }

    public int getScore() {
        return score;
    }

    public int getLife() {
        return life;
    }

    public int getTrail() {
        return currentTrial;
    }

    public int getRound() {
        return round;
    }

    //检查当前游戏状态
    //检查当前trail是否足以进入下一 round
    //检查当前round是否足够结束游戏
    public void checkStatus() {
        //此时的分数大于设置的阈值，游戏进入下一阶段，分数清零重新计算
        if (score >= scoreOfRound[round]) {
            currentTrial++;
            score = 0;
            
            //当游戏的trail大于3时进入下一 round
            if (currentTrial >= 3) {
                round++;
                life = 100;//当游戏进入到新的round生命值回复
                if (round >= 3) winGame();
                currentTrial = 0;              
                actionManager.setRound(round);
            }
        }
    }

    //判断飞碟是否已经离开视野
    private bool outOfSight(Vector3 pos) {
        return pos.x > 35 || pos.x < -35
            || pos.y > 35 || pos.y < -35
            || pos.z > 10 || pos.z < -300;
    }

    //检查当前所有被使用的飞碟是否已经飞出视野
    //将飞出视野的飞碟“销毁”
    //每销毁一个飞碟就将当前飞碟数量减一，游戏将自动补齐缺少的飞碟
    private void removeRemoteDisk() {
        for (int i = 0; i < diskCount; i++) {
            GameObject tmp = currentDisks[i].disk;
            if (outOfSight(tmp.transform.position)) {
                tmp.SetActive(false);
                diskFactory.FreeDisk(currentDisks[i]);
                actionManager.freeAction(currentDisks[i]);
                currentDisks.Remove(currentDisks[i]);
                diskCount--;
                life--;
            }
        }
    }

    //发送飞碟
    private void sendDisk() {
        diskCount++;
        DiskModel disk = diskFactory.getDisk(); //从工厂获取新的飞碟
        currentDisks.Add(disk);                 //将新飞碟加入到当前的列表
        actionManager.UFOFly(disk);             //令飞碟进行移动
    }

    //检查玩家是否射中飞碟
    public void Hit(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        DiskModel hitDisk;

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            hitDisk = diskFactory.findDisk(hit.collider.gameObject.GetInstanceID());

            //射线打中物体
            if (hitDisk != null) {
                score += hitDisk.score;

                //显示爆炸粒子效果
                hitDisk.disk.GetComponent<ParticleSystem>().Play();

                //等0.5秒后执行回收飞碟
                StartCoroutine(WaitingParticle(0.50f, diskFactory, hitDisk));
                break;
            }
        }
    }

    public void winGame() {
        playing = false;
        Debug.Log("you win");
    }

    //暂停几秒后回收飞碟
    IEnumerator WaitingParticle(float wait_time, DiskFactory diskFactory, DiskModel hitDisk) {
        yield return new WaitForSeconds(wait_time);
        if (hitDisk.disk.active == true) {
            hitDisk.disk.SetActive(false);
            hitDisk.disk.GetComponent<ParticleSystem>().Stop();
            hitDisk.disk.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hitDisk.disk.GetComponent<Rigidbody>().useGravity = false;
            currentDisks.Remove(hitDisk);
            actionManager.freeAction(hitDisk);
            diskFactory.FreeDisk(hitDisk);
            diskCount--;
        }
    }
}
