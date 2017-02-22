using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : ParticleEffects {
    //All public variables uses prefab, no need to put objects in scene
    public GameObject quickAttack;
    public GameObject heavyAttack;
    public GameObject quickAttackHitSpark;
    public GameObject generalHitSpark;
    public GameObject dashAfterImage;
    public GameObject jumpDust;
    public GameObject runningDust;
    public GameObject landingDust;
    public GameObject heal;
    public GameObject fireDamage;

    //public GameObject heavyHitImpact;

    protected override void ChildSpecificAwake() //Awake since other scripts will need the variables here at start
    {
        InstantiateParticles();
        ClassSpecificAwake();
    }

    protected virtual void ClassSpecificAwake()
    {
        //Use this to add anything for the awake of children
    }

    void InstantiateParticles()
    {
        InstantiateParticle(ref generalHitSpark);
        InstantiateParticle(ref quickAttack);
        InstantiateParticle(ref heavyAttack);
        InstantiateParticle(ref dashAfterImage);
        InstantiateParticle(ref jumpDust);
        InstantiateParticle(ref runningDust);
        InstantiateParticle(ref landingDust);
        InstantiateParticle(ref heal);
        InstantiateParticle(ref fireDamage);
    }

    public void PlayParticleQuickAttack()
    {
        PlayParticle(quickAttack);
    }

    public void PlayParticleHeavyAttack()
    {
        PlayParticle(heavyAttack);
    }

    public void PlayHitSpark(Vector3 location)
    {
        generalHitSpark.transform.position = location;
        PlayParticle(generalHitSpark);
    }

    public void PlayJumpDust()
    {
        PlayParticle(jumpDust);
    }

    public void PlayRunningDust()
    {
        PlayParticle(runningDust);
    }

    public void PlayLandingDust()
    {
        PlayParticle(landingDust);
    }

    public void PlayDashAfterImage(bool play)
    {
        if (play)
        {
            dashAfterImage.transform.localScale = transform.localScale;
            PlayParticle(dashAfterImage);
        }
        else
            dashAfterImage.GetComponent<ParticleSystem>().Stop();
    }

    public void PlayParticleHeal()
    {
        if(!(GetComponent<PlayerProperties>().currentHealth == GetComponent<PlayerProperties>().maxHealth))
            PlayParticle(heal);
    }
}
