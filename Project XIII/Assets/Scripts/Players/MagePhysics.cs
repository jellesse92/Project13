﻿using UnityEngine;
using System.Collections;

public class MagePhysics : PlayerPhysics
{
    const float QUICK_ORIGIN_X = 5f;
    const float QUICK_ORIGIN_Y = 2f;
    const float QUICK_ATTACK_DURATION = 0;

    const float HEAVY_ORIGIN_X = 5f;
    const float HEAVY_ORIGIN_Y = 0f;
    const float HEAVY_ATTACK_DURATION = 2f;

    const float HEAVY_AIR_ORIGIN_X = 5f;
    const float HEAVY_AIR_ORIGIN_Y = 3f;
    const float HEAVY_AIR_FORCE_X = 14000f;
    const float HEAVY_AIR_FORCE_Y = -15000f;
    const float HEAVY_AIR_ATTACK_DURATION = 1f;

    public GameObject quickAttackReticle;                   //Reticle for applying quick attack
    public GameObject heavyAttackReticle;                   //Reticle for applying heavy attack
    bool quickAttackActive = false;                         //Returns if light ground attack is running
    bool heavyAttackActive = false;                         //Returns if heavy ground attack is running

    public GameObject shieldParticle;                       //Particle effect and collider for shield burst

    public GameObject meteor;                               //Object for meteor
    bool heavyAirAttackActive = false;                      //Returns if heavy air attack is on coold down or not

    public override void ClassSpecificStart()
    {
        quickAttackReticle = (GameObject)Instantiate(quickAttackReticle);
        quickAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
        heavyAttackReticle = (GameObject)Instantiate(heavyAttackReticle);
        heavyAttackReticle.GetComponent<MageReticleScript>().SetMaster(this.gameObject);
        meteor = (GameObject)Instantiate(meteor);
        meteor.transform.GetChild(0).GetComponent<MageMeteorScript>().SetMaster(this.gameObject);
    }

    

    void ActivateQuickReticle()
    {
        if (!quickAttackActive)
        {
            ActivateReticle(quickAttackReticle, QUICK_ORIGIN_X, QUICK_ORIGIN_Y, ref quickAttackActive);
        }
    }

    void ActivateHeavyReticle()
    {
        if (!heavyAttackActive)
        {
            ActivateReticle(heavyAttackReticle, HEAVY_ORIGIN_X, HEAVY_ORIGIN_Y, ref heavyAttackActive);
        }
    }

    void ExecuteHeavyAirAttack()
    {
        if (heavyAirAttackActive)
            return;

        meteor.transform.position = transform.position + new Vector3(HEAVY_AIR_ORIGIN_X * transform.localScale.x, HEAVY_AIR_ORIGIN_Y, transform.position.z);
        meteor.transform.GetChild(0).GetComponent<MageMeteorScript>().ActivateAttack();
        meteor.GetComponent<Rigidbody2D>().AddForce(new Vector2(HEAVY_AIR_FORCE_X * transform.localScale.x, HEAVY_AIR_FORCE_Y));
        Invoke("EndAirHeavyAttack", HEAVY_AIR_ATTACK_DURATION);
        heavyAirAttackActive = true;
    }

    protected void ActivateReticle(GameObject reticle, float x, float y, ref bool attackActiveState)
    {
        reticle.transform.position = transform.position + new Vector3(x * transform.localScale.x, y, transform.position.z);
        reticle.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        reticle.GetComponent<SpriteRenderer>().enabled = true;
        reticle.GetComponent<Collider2D>().enabled = true;
        reticle.SetActive(true);
        attackActiveState = true;
    }

 

    void PlayChildrenParticles(GameObject obj)
    {
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        foreach (Transform child in obj.transform.GetChild(0))
            child.GetComponent<ParticleSystem>().Play();
    }

    void ExecuteQuickAttack()
    {
        if (quickAttackReticle.GetComponent<SpriteRenderer>().enabled)
        {
            quickAttackReticle.GetComponent<SpriteRenderer>().enabled = false;
            PlayChildrenParticles(quickAttackReticle);
            quickAttackReticle.GetComponent<MageReticleScript>().ReleaseQuickAttack();
            Invoke("EndQuickAttack", QUICK_ATTACK_DURATION);
        }

    }

    void ExecuteHeavyAttack()
    {
        if (heavyAttackReticle.GetComponent<SpriteRenderer>().enabled)
        {
            heavyAttackReticle.GetComponent<SpriteRenderer>().enabled = false;
            PlayChildrenParticles(heavyAttackReticle);
            heavyAttackReticle.GetComponent<MageReticleScript>().ReleaseHeavyAttack();
            Invoke("EndHeavyAttack", HEAVY_ATTACK_DURATION);
        }

    }

    void EndAttack(GameObject reticle, ref bool activeState)
    {
        reticle.GetComponent<MageReticleScript>().ExtinguishAttack();
        reticle.GetComponent<Collider2D>().enabled = false;
        activeState = false;
    }

    void EndHeavyAttack()
    {
        heavyAttackReticle.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        heavyAttackReticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        EndAttack(heavyAttackReticle, ref heavyAttackActive);
    }

    void EndQuickAttack()
    {
        quickAttackReticle.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        quickAttackReticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        EndAttack(quickAttackReticle, ref quickAttackActive);
    }

    void EndAirHeavyAttack()
    {
        meteor.transform.GetChild(0).GetComponent<MageMeteorScript>().Reset();
        heavyAirAttackActive = false;
    }

    void InterruptCast()
    {
        //If it's not already cast and runnning, deactivate the reticle and its children
        if (!quickAttackReticle.transform.GetChild(0).gameObject.activeSelf)
        {
            EndQuickAttack();
        }
    }

    void ActivateShield()
    {
        shieldParticle.GetComponent<ParticleSystem>().Play();
    }
}
