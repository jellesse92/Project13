﻿using UnityEngine;
using System.Collections;

public class SwordsmanMelee : MonoBehaviour {

    PlayerProperties playerProperties;
    PlayerSoundEffects playerSoundEffects;
    PlayerParticleEffects playerParticleEffects;
    int damage;

    void Start()
    {
        GetProperties();
    }

    void GetProperties() //thinking all attack will share this
    {
        playerProperties = transform.parent.GetComponent<PlayerProperties>();
        playerParticleEffects = transform.parent.GetComponent<PlayerParticleEffects>();
        playerSoundEffects = transform.parent.GetComponent<PlayerSoundEffects>();

        damage = playerProperties.GetPlayerStats().quickAttackStrength;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            playerSoundEffects.PlayHitSpark();
            playerParticleEffects.PlayHitSpark(col.GetComponent<Enemy>().GetCenter());
            col.GetComponent<Enemy>().Damage(damage, 1f);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(400f * transform.parent.localScale.x, 5000f));
        }

    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
