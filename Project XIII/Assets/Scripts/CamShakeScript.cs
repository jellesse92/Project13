using UnityEngine;
using System.Collections;

public class CamShakeScript : MonoBehaviour {
    //Credit: http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/


    Vector3 origin;

    float shakeAmount;


    void Start()
    {
        origin = transform.position;
        //StartShake(.1f);
    }

    public void StartShake(float magnitude)
    {
        shakeAmount = magnitude;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.3f);
    }



    void CameraShake()
    {
        float quakeAmt = Random.value * shakeAmount * 2 - shakeAmount;
        Vector3 pp = transform.position;
        pp.y += quakeAmt; // can also add to x and/or z
        transform.position = pp;
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        transform.position = origin;
    }
}
