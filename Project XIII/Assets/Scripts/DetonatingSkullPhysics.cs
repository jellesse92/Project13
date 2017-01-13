using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonatingSkullPhysics : EnemyPhysics {

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

    public override void PlayDeath()
    {
        base.PlayDeath();
        explosionRadius.GetComponent<Collider2D>().enabled = false;
    }

    public override void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    public void Explode()
    {
        explosionRadius.GetComponent<DetonatingEnemyExplosion>().ApplyExplosion();
    }


}
