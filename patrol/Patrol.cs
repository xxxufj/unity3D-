using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour{
    private int index;              //巡逻兵编号
    private GameObject patrol;      //gameobject
    Vector3 init;                   //巡逻兵的初始位置，巡逻兵每巡逻一圈回到初始位置
    private float range_x1, range_y1, range_x2, range_y2;   //巡逻兵巡逻范围
    const int distance = 38;        //巡逻长度最大值

    public Patrol(int _index, Vector3 initPos) {
        patrol = Object.Instantiate(Resources.Load<GameObject>("Patrol"), Vector3.zero, Quaternion.identity);
        patrol.name = _index.ToString();
        index = _index;
        init = initPos;
        patrol.transform.position = initPos;
        initBorder();
    }

    private void initBorder() {
        range_x1 = init.x + 8;
        range_y1 = init.z + 8;
        range_x2 = range_x1 - distance;
        range_y2 = range_y1 - distance;
    }

    //判断巡逻兵是否接近边界
    public bool OutOfBorder() {
        Vector3 pos = patrol.transform.position;
        return !(pos.x >= range_x2 && pos.x <= range_x1 && pos.z >= range_y2 && pos.z <= range_y1);
    }

    public int getIndex() {
        return index;
    }

    public GameObject getGameObject() {
        return patrol;
    }

    public Vector3 getInit() {
        return init;
    }
}
