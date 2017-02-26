using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySineBullet : EnemyProjectile {

    //SINE WAVE PATTERN SCRIPT CREDIT: http://answers.unity3d.com/questions/803434/how-to-make-projectile-to-shoot-in-a-sine-wave-pat.html

    string FIREBALL_NAME = "Fireball";
    string IMPACT_NAME = "Impact";

    const float BULLET_SPEED = 10f;

    public AudioClip impactAudio;

    ParticleSystem fireballParticle;
    ParticleSystem impactParticle;

    public bool positiveStart = true;               //Travels upwards first if true
    public float moveSpeed = 5f;                    //Speed which it moves
    public float frequency = 20f;                   //Speed of sine movement
    public float magnitude = .5f;                   //Size of the sine movement
    bool fired = false;
    float directionMultiplier = 1;
    bool firstShotFired = false;

    Vector3 pos;
    Vector3 axis;
    float timeStart = 0f;

    AudioSource myAudio;

    // Use this for initialization
    public override void Start () {
        base.Start();
        myAudio = GetComponent<AudioSource>();

        if (transform.Find(FIREBALL_NAME))
            fireballParticle = transform.Find(FIREBALL_NAME).GetComponent<ParticleSystem>();
        if (transform.Find(IMPACT_NAME))
            impactParticle = transform.Find(IMPACT_NAME).GetComponent<ParticleSystem>();

        pos = transform.position;
        axis = -transform.up;
    }

    private void Update()
    {
        if(!fired)
            return;

        if(Time.timeScale != 0f)
        {
            pos += (transform.right * Time.deltaTime * moveSpeed) * directionMultiplier;
            transform.position = pos + axis * Mathf.Sin(timeStart * frequency) * magnitude;

            timeStart += Time.deltaTime;
        }
    } 

    public override void Fire(float x, float y, bool faceRight = true)
    {
        if (positiveStart)
            axis = -transform.up;
        else
            axis = transform.up;

        if (faceRight)
            directionMultiplier = 1f;
        else
            directionMultiplier = -1f;

        timeStart = 0f;

        pos = transform.position;
        fired = true;
        firstShotFired = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        fired = false;
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
