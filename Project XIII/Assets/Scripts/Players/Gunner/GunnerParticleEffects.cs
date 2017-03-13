using UnityEngine;
using System.Collections;

public class GunnerParticleEffects : PlayerParticleEffects
{
    public GameObject chargingParticles;
    public GameObject chargingFirstCharge;
    public GameObject chargingSecondCharge;

    Vector3 positionJumpDust;
    Vector3 positionRunningDust;
    Vector3 positionLandingDust;

    protected override void ClassSpecificAwake()
    {
        positionJumpDust = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        positionRunningDust = new Vector3(transform.position.x - 0.5f, transform.position.y - 3f, transform.position.z);
        positionLandingDust = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);

        InstantiateParticle(ref chargingParticles);
        InstantiateParticle(ref chargingFirstCharge);
        InstantiateParticle(ref chargingSecondCharge);

        ChangeParticlePosition(ref jumpDust, positionJumpDust);
        ChangeParticlePosition(ref runningDust, positionRunningDust);
        ChangeParticlePosition(ref landingDust, positionLandingDust);

        GunnerAdjustment();
    }

    void GunnerAdjustment()
    {
        if (quickAttack)
            quickAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y-.5f, transform.position.z);
        if (heavyAttack)
            heavyAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y-.5f, transform.position.z);
    }

    public void PlayChargingDust(bool play)
    {
        Debug.Log("Charging" + play);
        if (play)
        {
            chargingParticles.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            chargingParticles.GetComponent<ParticleSystem>().Stop();
            chargingParticles.GetComponent<ParticleSystem>().Clear();
        }
    }


}
