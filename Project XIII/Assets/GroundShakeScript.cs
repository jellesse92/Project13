using UnityEngine;
using System.Collections;

public class GroundShakeScript : MonoBehaviour {

    CamShakeScript shakeScript;

    void Start()
    {
        shakeScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamShakeScript>();
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            float v = col.gameObject.GetComponent<Rigidbody2D>().velocity.y;

            if (v < -20f)
            {
                shakeScript.StartShake(-.005f * v % 20f,.1f);
            }
        }
    }
    

}
