﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIController : MonoBehaviour {

    public Boss bossScript;
    public Image healthBar;
    Animator myAnimator;

	// Use this for initialization
	void Start () {
        myAnimator = GetComponent<Animator>();
        bossScript.SetHealthBar(healthBar);

        Debug.Log("TEMPORARY. SET TO SCRIPTED EVENT");
        Invoke("LoadHealthBar", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadHealthBar()
    {
        myAnimator.SetTrigger("load");
    }

    void ActivateBoss()
    {
        bossScript.Unfreeze();
    }

}
