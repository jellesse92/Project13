using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanParticleEffects : PlayerParticleEffects {
    public GameObject chargingDust;

    protected override void ClassSpecificAwake()
    {
        InstantiateParticle(ref chargingDust);
        if(chargingDust)
            chargingDust.transform.position = new Vector3(transform.position.x + 0.7f, transform.position.y - 2.5f, transform.position.z);
    }

    public void PlayChargingDust(bool play)
    {
        if (play)
        {
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = chargingDust.GetComponent<ParticleSystem>().velocityOverLifetime;
            ParticleSystem.MinMaxCurve rate = velocityOverLifetime.x;

            if (transform.localScale.x < 0)
                rate.constantMax = rate.constantMax * -1;
            else
                rate.constantMax = Mathf.Abs(rate.constantMax);

            velocityOverLifetime.x = rate;
            chargingDust.GetComponent<ParticleSystem>().Play();
        }
        else
            chargingDust.GetComponent<ParticleSystem>().Stop();
    }
}
