using UnityEngine;
using System.Collections;
using Vuforia;
using System;

public class MyVBHandler : MonoBehaviour, IVirtualButtonEventHandler {
    Material m1;
    GameObject button;
    GameObject sun;
    bool normalSize;
    Vector3 initSize;

    public void OnButtonPressed(VirtualButtonBehaviour vb) {
        m1.color = Color.red;
        print("VBPressed    " + vb.VirtualButton.Area.leftTopX);
        button.transform.localScale = new Vector3(1.5f, 0.08f, 1.5f);
        if (normalSize == true) {
            normalSize = false;
            sun.transform.localScale = sun.transform.localScale * 1.5f;
        }
        else {
            normalSize = true;
            sun.transform.localScale = initSize;
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        button.transform.localScale = new Vector3(1f, 0.05f, 1f);
        m1.color = Color.white;
    }

    // Use this for initialization
    void Start() {
        normalSize = true;
        button = GameObject.FindWithTag("button");
        sun = GameObject.FindWithTag("Player");
        initSize = sun.transform.localScale;
        m1 = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        m1.color = Color.white;
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i) {
            vbs[i].RegisterEventHandler(this);//把ImageTarget下所有含有VirtualButtonBehaviour组件的物体注册过来（使用前面写的Pressed和Released方法处理）。
        }
    }
}
