using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskModel{
    public GameObject disk;
    public int type;    //飞碟的类型，根据类型实例化预制的飞碟
    public int ID;      //飞碟的标识
    public int score;   //击中飞碟后可得的分数
    //根据飞碟类型和标识ID来创建飞碟
    public DiskModel(int type, int ID) {
        disk = Object.Instantiate(Resources.Load<GameObject>("disk"+type.ToString()), Vector3.zero, Quaternion.identity);
        this.type = type;
        this.ID = ID;
        score = type + 1;
    }
    public int getDiskID() {
        return ID;
    }

    public void setDiskID(int ID) {
        this.ID = ID;
    }

    public int getType() {
        return type;
    }
}

public class DiskFactory : MonoBehaviour {
    private int diskID = 0;
    private List<DiskModel>[] disks = new List<DiskModel>[3]; //共有三种飞碟
    bool[,] diskStatus = new bool[3, 20];   //每种飞碟最多有二十个
    int[] size = new int[3];    //保存当前已创建出来的每种飞碟的数量

    private List<DiskModel> ddisk = new List<DiskModel>();

    public void Start() {
        for (int i = 0; i < 3; i++) {
            size[i] = 0;                                 
            disks[i] = new List<DiskModel>();
        }
        for (int j = 0; j < 3; j++) {
            for (int i = 0; i < 20; i++) {
                diskStatus[j, i] = true;
            }
        }
    }

    //随机获取一种空闲飞碟，如果飞碟不足则新建一个实例
    public DiskModel getDisk() {

        //随机从三种预制中选择一个飞碟外观
        int type = Random.Range(0, 3);
        DiskModel disk;

        //尝试获取已经被创建但目前处于空闲态的飞碟
        for (int i = 0; i < size[type]; i++) {
            if (diskStatus[type, i] == false) {
                diskStatus[type, i] = true;
                disks[type][i].disk.SetActive(true);
                disk = disks[type][i];
                disk.setDiskID(diskID);
                diskID++;
                //取出时飞碟不能够有爆炸特效
                disk.disk.GetComponent<ParticleSystem>().Stop();
                return disk;
            }
        }

        //当前没有可用的空闲飞碟，需要创建
        disk = new DiskModel(type, diskID);
        diskID++;
        disks[type].Add(disk);
        diskStatus[type, size[type]] = true;
        size[type]++;
        disk.disk.GetComponent<ParticleSystem>().Stop();
        return disk;
    }

    //回收飞碟
    public void FreeDisk(DiskModel disk) {
        int type = disk.getType();
        int ID = disk.getDiskID();

        for (int i = 0; i < size[type]; i++) {
            if (disk.getDiskID() == disks[type][i].getDiskID()) {
                diskStatus[type, i] = false;
                return;
            }
        }
    }

    //根据 gameobject 的 instanceID 来查找飞碟
    public DiskModel findDisk(int InstanceID) {
        for(int i = 0; i < 3; i++) {
            for(int j = 0; j < size[i]; j++) {
                if (disks[i][j].disk.GetInstanceID() == InstanceID) return disks[i][j];
            }
        }
        return null;
    }
}

