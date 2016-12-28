using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanParticleEffects : PlayerParticleEffects {
    public GameObject chargingDust;
    public GameObject upAttackDust;

    Vector3 positionChargingDust;
    Vector3 positionJumpDust;
    Vector3 positionRunningDust;
    Vector3 positionUpAttackDust;
    Vector3 positionUpAttack;

    protected override void ClassSpecificAwake()
    {
        positionChargingDust = new Vector3(transform.position.x + 0.7f, transform.position.y - 2.5f, transform.position.z);
        positionJumpDust = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionRunningDust = new Vector3(transform.position.x - 0.5f, transform.position.y - 2.5f, transform.position.z);
        positionUpAttackDust = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionUpAttack = new Vector3(transform.position.x + 0.7f, transform.position.y - 2.5f, transform.position.z);

        InstantiateParticle(ref chargingDust);
        InstantiateParticle(ref upAttackDust);

        if (chargingDust)
            chargingDust.transform.position = positionChargingDust;
        if (jumpDust)
            jumpDust.transform.position = positionJumpDust;
        if (runningDust)
            runningDust.transform.position = positionRunningDust;
        if (upAttackDust)
            upAttackDust.transform.position = positionUpAttackDust;
        if (quickAttack)
            quickAttack.transform.position = positionUpAttack;
    }

    public void PlayChargingDust(bool play)
    {
        if (play)
            chargingDust.GetComponent<ParticleSystem>().Play();
        else
            chargingDust.GetComponent<ParticleSystem>().Stop();
    }

    public void PlayUpAttackDust()
    {
        PlayParticle(upAttackDust);
    }
}
