using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanParticleEffects : PlayerParticleEffects {
    public GameObject chargingDust;

    Vector3 positionChargingDust;
    Vector3 positionJumpDust;
    Vector3 positionRunningDust;

    protected override void ClassSpecificAwake()
    {
        positionChargingDust = new Vector3(transform.position.x + 0.7f, transform.position.y - 2.5f, transform.position.z);
        positionJumpDust = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);
        positionRunningDust = new Vector3(transform.position.x - 0.5f, transform.position.y - 2.5f, transform.position.z);
        InstantiateParticle(ref chargingDust);
        if (chargingDust)
            chargingDust.transform.position = positionChargingDust;
        if (jumpDust)
            jumpDust.transform.position = positionJumpDust;
        if (runningDust)
            runningDust.transform.position = positionRunningDust;
    }

    public void PlayChargingDust(bool play)
    {
        if (play)
            chargingDust.GetComponent<ParticleSystem>().Play();
        else
            chargingDust.GetComponent<ParticleSystem>().Stop();
    }
}
