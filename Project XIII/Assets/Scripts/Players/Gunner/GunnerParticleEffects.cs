using UnityEngine;
using System.Collections;

public class GunnerParticleEffects : PlayerParticleEffects
{
    protected override void ClassSpecificAwake()
    {
        GunnerAdjustment();
    }

    void GunnerAdjustment()
    {
        if (quickAttack)
            quickAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
        if (heavyAttack)
            heavyAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
    }
}
