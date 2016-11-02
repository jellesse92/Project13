﻿using UnityEngine;
using System.Collections;

public class CamShakeScript : MonoBehaviour {
    //Credit: http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/

    float shakeAmount;
    bool isShaking = false;

    public void StartShake(float magnitude)
    {
        shakeAmount = magnitude;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", .5f);
    }



    void CameraShake()
    {
        isShaking = true;

        float quakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;
        float quakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
        Vector3 pp = transform.position;
        pp.y += quakeAmtY;
        pp.x += quakeAmtX;
        transform.position = pp;
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        isShaking = false;
        transform.position = transform.parent.position;
    }

    public bool GetIsShaking()
    {
        return isShaking;
    }
}