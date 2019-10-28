using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flare : MonoBehaviour {
    ParticleSystem particleSystem;
    ParticleSystem.ForceOverLifetimeModule forceMode;
    ParticleSystem.ColorOverLifetimeModule colorMode;
    int windPower;

    // Use this for initialization
    void Start() {
        particleSystem = GetComponent<ParticleSystem>();
        forceMode = particleSystem.forceOverLifetime;
        colorMode = particleSystem.colorOverLifetime;
    }

    // Update is called once per frame
    void Update() {
        if (windPower >= 10 || windPower <= -10) {
            putOut();
        }
    }

    void putOut() {
        ParticleSystem.MainModule main = particleSystem.main;
        main.loop = false;
        main.startLifetime = 2f;
        main.startSize = 2f;
        main.startSpeed = 2f;
        forceMode.x = 0;

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.black, 2.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 2.0f) });
        colorMode.color = grad;
    }

    public void increaseFlame() {
        particleSystem.startSize = particleSystem.startSize * 1.1f;
        particleSystem.startLifetime = particleSystem.startLifetime * 1.1f;
    }

    public void decreaseFlame() {
        particleSystem.startSize = particleSystem.startSize * 0.9f;
        particleSystem.startLifetime = particleSystem.startLifetime * 0.9f;
    }

    public void increaseWindPower() {
        windPower++;
        ParticleSystem.MinMaxCurve temp = forceMode.x;
        temp.constantMax += -1.2f;
        forceMode.x = temp;
    }

    public void decreaseWindPower() {
        windPower--;
        ParticleSystem.MinMaxCurve temp = forceMode.x;
        temp.constantMax += 1.2f;
        forceMode.x = temp;
    }
}