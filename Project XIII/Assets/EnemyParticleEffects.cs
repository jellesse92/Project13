using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticleEffects : ParticleEffects
{
    public GameObject deathParticles;
    public GameObject coins;

    protected override void ChildSpecificAwake()
    {
        InstantiateParticle(ref deathParticles);
        InstantiateParticle(ref coins);
    }
}
