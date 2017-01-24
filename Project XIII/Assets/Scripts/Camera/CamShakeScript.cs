using UnityEngine;
using System.Collections;

public class CamShakeScript : MonoBehaviour {
    //Credit: http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/

    float shakeAmount;
    bool isShaking = false;
    bool keepShaking = true;

    void Start()
    {
    }

    void FixedUpdate()
    {

    }

    public IEnumerator InfiniteShake()
    {
        while (true)
        {
            StartShake(.02f);
            yield return new WaitForSeconds(3f);
        }
    }

    public void StartShake(float magnitude, float duration = .5f)
    {
        shakeAmount = magnitude;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", duration);
    }



    void CameraShake()
    {
        isShaking = true;

        float quakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;
        float quakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
        Vector3 pp = transform.parent.position;
        pp.y += quakeAmtY;
        pp.x += quakeAmtX;
        pp.z = transform.position.z;
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
