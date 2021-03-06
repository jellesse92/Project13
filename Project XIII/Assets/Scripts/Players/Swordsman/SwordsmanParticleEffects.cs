﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanParticleEffects : PlayerParticleEffects {
    public GameObject chargingDust;
    public GameObject chargingParticles;
    public GameObject chargingFirstCharge;
    public GameObject chargingSecondCharge;
    public GameObject chargingTrail;

    public GameObject dashTrail;
    public GameObject upAttackDust;
    public GameObject finisherHitSpark;
    
    Vector3 positionChargingDust;
    Vector3 positionChargingTrail;

    Vector3 positionDashTrail;
    Vector3 positionJumpDust;
    Vector3 positionRunningDust;
    Vector3 positionLandingDust;

    Vector3 positionUpAttackDust;
    Vector3 positionUpAttack;

    protected override void ClassSpecificAwake()
    {
        positionChargingDust = new Vector3(transform.position.x + 0.7f, transform.position.y - 2.5f, transform.position.z);
        positionJumpDust = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionRunningDust = new Vector3(transform.position.x - 0.5f, transform.position.y - 2.5f, transform.position.z);
        positionLandingDust = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionUpAttackDust = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionUpAttack = new Vector3(transform.position.x + 0.7f, transform.position.y - 2.5f, transform.position.z);
        positionChargingTrail = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionDashTrail = new Vector3(transform.position.x + 1f, transform.position.y - 1.5f, transform.position.z);

        InstantiateParticle(ref chargingDust);
        InstantiateParticle(ref chargingParticles);
        InstantiateParticle(ref chargingFirstCharge);
        InstantiateParticle(ref chargingSecondCharge);
        InstantiateParticle(ref upAttackDust);
        InstantiateParticle(ref finisherHitSpark);
        InstantiateParticle(ref chargingTrail);
        InstantiateParticle(ref dashTrail);

        ChangeParticlePosition(ref chargingDust, positionChargingDust);
        ChangeParticlePosition(ref jumpDust, positionJumpDust);
        ChangeParticlePosition(ref runningDust, positionRunningDust);
        ChangeParticlePosition(ref landingDust, positionLandingDust);
        ChangeParticlePosition(ref upAttackDust, positionUpAttackDust);
        ChangeParticlePosition(ref quickAttack, positionUpAttack);
        ChangeParticlePosition(ref chargingTrail, positionChargingTrail);
        ChangeParticlePosition(ref dashTrail, positionDashTrail);

    }

    public void PlayChargingDust(bool play)
    {
        if (play)
        {
            chargingDust.GetComponent<ParticleSystem>().Play();
            chargingParticles.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            chargingDust.GetComponent<ParticleSystem>().Stop();
            chargingParticles.GetComponent<ParticleSystem>().Stop();
            chargingParticles.GetComponent<ParticleSystem>().Clear();

        }
    }

    public void PlayChargingTrail(bool play)
    {
        if (play)
            chargingTrail.GetComponent<ParticleSystem>().Play();
        else
            chargingTrail.GetComponent<ParticleSystem>().Stop();
    }

    public void PlayDashTrail(bool play)
    {
        if (play)
            dashTrail.GetComponent<ParticleSystem>().Play();
        else
            dashTrail.GetComponent<ParticleSystem>().Stop();
    }

    public void PlayUpAttackDust()
    {
        PlayParticle(upAttackDust);
    }
    
    public void PlayFinisherHitSpark(Vector3 location)
    {
        finisherHitSpark.transform.position = location;
        PlayParticle(finisherHitSpark);
    }
}
