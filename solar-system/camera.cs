using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {
    private GameObject camera0, camera1, camera2;
    public GameObject earth;
    //public Texture2D img;

    // Start is called before the first frame update
    void Start() {
        camera0 = GameObject.Find("Main Camera");
        camera1 = GameObject.Find("Earth Camera");
        CloseCameras();
        camera0.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        camera1.transform.position = earth.transform.position + new Vector3(0, 0, 0);
        camera1.transform.eulerAngles = earth.transform.eulerAngles;
    }
    public void CloseCameras() {
        camera1.SetActive(false);
        camera0.SetActive(false);
    }

    void OnGUI() {

        if (GUI.Button(new Rect(0, 0, 50, 50), "Main")) {
            camera1.SetActive(false);
            camera0.SetActive(true);
        }

        if (GUI.Button(new Rect(0, 60, 50, 50), "Earth")) {
            camera0.SetActive(false);
            camera1.SetActive(true);
        }
     
    }
}
