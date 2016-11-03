﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class GunnerStats
{
    public float quickBulletSpeed = 1f;
    public float heavyBulletSpeed = 1f;
    public float heavyAttackCoolDown = 1.2f;
    public float dodgeCoolDown = 1.0f;
    public float heavyAttackKnockBackForce = 10f;
}

[System.Serializable]
public class GunnerBullets
{
    public GameObject quickBullets;
    public GameObject heavyBullets;
}
public class GunnerProperties : PlayerProperties{

    public GunnerStats gunnerStats;
    public GunnerBullets gunnerbullets;
    Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.runtimeAnimatorController.animationClips[8].frameRate = 60;
        psScript = GameObject.FindGameObjectWithTag("In Game Status Panel"); //Do this for other classes
        gunnerbullets.quickBullets = Instantiate(gunnerbullets.quickBullets);
        gunnerbullets.heavyBullets = Instantiate(gunnerbullets.heavyBullets);
    }
    public GunnerStats GetGunnerStats()
    {
        return gunnerStats;
    }

    public GunnerBullets GetGunnerBullets()
    {
        return gunnerbullets;
    }

}
