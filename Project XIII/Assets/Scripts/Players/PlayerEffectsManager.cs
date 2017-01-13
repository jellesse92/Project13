using UnityEngine;
using System.Collections;

public class PlayerEffectsManager : MonoBehaviour {

    GameObject screenFlash;
    GameObject screenShake;
    GameObject deathScreen;
    GameObject musicManager;

	// Use this for initialization
	void Start () {
        screenFlash = GameObject.FindGameObjectWithTag("Hit Flash Image");
        screenShake = GameObject.FindGameObjectWithTag("MainCamera");
        deathScreen = GameObject.FindGameObjectWithTag("Death Screen");
        musicManager = GameObject.FindGameObjectWithTag("Music");
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

    public bool ReportLastDeath()
    {
        if(musicManager != null)
        {
            musicManager.GetComponent<MusicManager>().PlayDeathMusic();
        }

        if(deathScreen != null)
        {
            foreach(Transform child in transform)
            {
                if (child.gameObject.activeSelf && child.GetComponent<PlayerProperties>().alive)
                    return false;
            }
            deathScreen.GetComponent<DeathScreenScript>().TriggerDeath();
            return true;
        }

        return false;
    }
}
