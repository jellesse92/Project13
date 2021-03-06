﻿using UnityEngine;
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

    public void DamageFlashScreen()
    {
        screenFlash.GetComponent<Animator>().SetTrigger("damageFlash");
    }
    //Gets the screen to shake
    public void ScreenShake(float magnitude = 0.2f, float duration = 1f)
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
            screenShake.GetComponent<CamShakeScript>().StopShake();
            deathScreen.GetComponent<DeathScreenScript>().TriggerDeath();
            return true;
        }

        return false;
    }
}
