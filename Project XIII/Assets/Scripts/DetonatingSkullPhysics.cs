using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonatingSkullPhysics : EnemyPhysics {

    const float EXPLOSION_DELAY = 1.5f;               //Amount of time enemy delays explosion after contact with player

    public GameObject explosionRadius;

    protected override void EnemySpecificStart()
    {
        explosionRadius.GetComponent<DetonatingEnemyExplosion>().SetDamage(attackPower);
    }

    public override void Reset()
    {
        base.Reset();
        explosionRadius.GetComponent<DetonatingEnemyExplosion>().Reset();
        explosionRadius.GetComponent<Collider2D>().enabled = true;
    }

    public float GetExplosionDelay()
    {
        return EXPLOSION_DELAY;
    }

    public void CancelExplosion()
    {
        explosionRadius.GetComponent<DetonatingEnemyExplosion>().CancelExplosion();
    }


}
