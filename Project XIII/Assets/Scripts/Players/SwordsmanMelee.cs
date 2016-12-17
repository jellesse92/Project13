using UnityEngine;
using System.Collections;

public class SwordsmanMelee : MonoBehaviour {

    int damage;
    PlayerSoundEffects playerSoundEffects;
    ParticleSystem hitSparkParticle;

    void Start()
    {
        getProperties();
    }

    void getProperties() //thinking all attack will share this
    {
        PlayerProperties playerProperties = transform.parent.GetComponent<PlayerProperties>();
        PlayerParticleEffects playerParticleEffects = transform.parent.GetComponent<PlayerParticleEffects>();
        PlayerStats playerstat = playerProperties.GetPlayerStats();

        playerSoundEffects = transform.parent.GetComponent<PlayerSoundEffects>();
        hitSparkParticle = playerParticleEffects.GetHitSpark();
        damage = playerstat.heavyAttackStrength;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<Enemy>().Damage(damage, 1f, hitSparkParticle);
            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(400f * transform.parent.localScale.x, 5000f));
        }

    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
