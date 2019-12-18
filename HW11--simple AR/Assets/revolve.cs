using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revolve : MonoBehaviour
{
    public float speed;
    public GameObject center;
    private int offset_x;//法平面的偏移角x
    private int offset_y;//法平面的偏移角y
    // Start is called before the first frame update
    void Start()
    {
        offset_x = Random.Range(10, 20);
        offset_y = Random.Range(40, 60);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.transform.position, new Vector3(offset_x, offset_y, 0), speed * Time.deltaTime);
    }
}
