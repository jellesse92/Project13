using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySineBullet : EnemyProjectile {

    string FIREBALL_NAME = "Fireball";
    string IMPACT_NAME = "Impact";

    const float BULLET_SPEED = 10f;

    public AudioClip impactAudio;

    ParticleSystem fireballParticle;
    ParticleSystem impactParticle;

    public bool positiveStart = true;               //Travels upwards first if true

    AudioSource myAudio;

    // Use this for initialization
    public override void Start () {
        base.Start();
        myAudio = GetComponent<AudioSource>();

        if (transform.Find(FIREBALL_NAME))
            fireballParticle = transform.Find(FIREBALL_NAME).GetComponent<ParticleSystem>();
        if (transform.Find(IMPACT_NAME))
            impactParticle = transform.Find(IMPACT_NAME).GetComponent<ParticleSystem>();
    }

    public override void Fire(float x, float y)
    {
        GetComponent<Rigidbody2D>().velocity = -(transform.position - new Vector3(x, y, transform.position.z)).normalized * BULLET_SPEED;
    }

    public override void PlayParticle(bool play)
    {
        if (play)
            fireballParticle.Play();
        else
        {
            fireballParticle.Stop();
            impactParticle.Play();
            myAudio.PlayOneShot(impactAudio);
        }
    }
}
