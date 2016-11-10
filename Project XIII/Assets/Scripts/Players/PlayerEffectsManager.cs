using UnityEngine;
using System.Collections;

public class PlayerEffectsManager : MonoBehaviour {

    GameObject screenFlash;
    GameObject screenShake;

	// Use this for initialization
	void Start () {
        screenFlash = GameObject.FindGameObjectWithTag("Hit Flash Image");
        screenShake = GameObject.FindGameObjectWithTag("MainCamera");
    }

    //Gets the screen to flash
    public void FlashScreen()
    {
        screenFlash.GetComponent<Animator>().SetTrigger("hitFlash");
    }

    //Gets the screen to shake
    public void ScreenShake(float magnitude, float duration = .5f)
    {
        screenShake.GetComponent<CamShakeScript>().StartShake(magnitude,duration);
    }
}
