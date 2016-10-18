﻿using UnityEngine;
using System.Collections;

public class CamShakeScript : MonoBehaviour {
    //Credit: http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/


    Vector3 origin;

    float shakeAmount;


    void Start()
    {
        origin = transform.position;
    }

    public void StartShake(float magnitude)
    {
        shakeAmount = magnitude;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", .5f);
    }



    void CameraShake()
    {
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
        transform.position = origin;
    }
}