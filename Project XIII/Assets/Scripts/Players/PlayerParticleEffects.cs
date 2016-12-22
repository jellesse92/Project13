﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour {
    //All public variables uses prefab, no need to put objects in scene
    public GameObject quickAttack;
    public GameObject heavyAttack;
    public GameObject quickAttackHitSpark;
    public GameObject generalHitSpark;
    public GameObject dashAfterImage;
    GameObject particlesHolder;
    //public GameObject heavyHitImpact;

    void Awake() //Awake since other scripts will need the variables here at start
    {
        InstantiateParticles();
        ClassSpecificAwake();
    }

    protected virtual void ClassSpecificAwake()
    {
        //Use this to add anything for the awake of children
    }
    void InstantiateParticles()
    {
        particlesHolder = new GameObject();
        particlesHolder.name = "Particles";
        particlesHolder.transform.parent = transform;

        InstantiateParticle(ref generalHitSpark);
        InstantiateParticle(ref quickAttack);
        InstantiateParticle(ref heavyAttack);
        InstantiateParticle(ref dashAfterImage);
    }

    protected void InstantiateParticle(ref GameObject particle)
    {
        if (particle)
        {
            particle = Instantiate(particle);
            particle.transform.position = transform.position;
            particle.transform.parent = particlesHolder.transform;
        }
    }

    public void PlayParticleQuickAttack()
    {
        quickAttack.GetComponent<ParticleSystem>().Play();
    }

    public void PlayParticleHeavyAttack()
    {
        heavyAttack.GetComponent<ParticleSystem>().Play();
    }

    public void PlayHitSpark(Vector3 location)
    {
        generalHitSpark.transform.position = location;
        generalHitSpark.GetComponent<ParticleSystem>().Play();
    }

    public void PlayDashAfterImage(bool play)
    {
        if (play)
        {
            dashAfterImage.transform.localScale = transform.localScale;
            dashAfterImage.GetComponent<ParticleSystem>().Play();
        }
        else
            dashAfterImage.GetComponent<ParticleSystem>().Stop();
    }
}
