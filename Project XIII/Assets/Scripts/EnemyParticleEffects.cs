using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticleEffects : ParticleEffects
{
    //public GameObject deathParticles;
    public GameObject coins;
    public GameObject fireDamage;

    protected override void ChildSpecificAwake()
    {
        //InstantiateParticle(ref deathParticles);
        InstantiateParticle(ref coins);
        InstantiateParticle(ref fireDamage);    
    }
    
    public void SpawnCoins(int amount)
    {
        coins.GetComponent<ParticleSystem>().Emit(amount);
    }
}
