using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanParticleEffects : PlayerParticleEffects {
    public GameObject chargingDust;
    public GameObject chargingParticles;
    public GameObject chargingExplosion;

    public GameObject upAttackDust;
    public GameObject finisherHitSpark;
    
    Vector3 positionChargingDust;
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

        InstantiateParticle(ref chargingDust);
        InstantiateParticle(ref chargingParticles);
        InstantiateParticle(ref chargingExplosion);
        InstantiateParticle(ref upAttackDust);
        InstantiateParticle(ref finisherHitSpark);

        ChangeParticlePosition(ref chargingDust, positionChargingDust);
        ChangeParticlePosition(ref jumpDust, positionJumpDust);
        ChangeParticlePosition(ref runningDust, positionRunningDust);
        ChangeParticlePosition(ref landingDust, positionLandingDust);
        ChangeParticlePosition(ref upAttackDust, positionUpAttackDust);
        ChangeParticlePosition(ref quickAttack, positionUpAttack);
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

    public void PlayUpAttackDust()
    {
        PlayParticle(upAttackDust);
    }

    public void PlayChargingExplosion()
    {
        PlayParticle(chargingExplosion);
    }

    public void PlayFinisherHitSpark(Vector3 location)
    {
        finisherHitSpark.transform.position = location;
        PlayParticle(finisherHitSpark);
    }
}
