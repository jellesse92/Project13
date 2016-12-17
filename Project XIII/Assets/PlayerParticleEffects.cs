using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour {
    //All public variables uses prefab, no need to put objects in scene
    public ParticleSystem quickAttack;
    public ParticleSystem heavyAttack;
    public ParticleSystem quickAttackHitSpark;
    public ParticleSystem generalHitSpark;
    //public GameObject heavyHitImpact;

    void Start()
    {
        InstantiateParticles();
    }

    void InstantiateParticles()
    {
        if (generalHitSpark)
        {
            generalHitSpark = Instantiate(generalHitSpark);
            generalHitSpark.transform.parent = transform;
        }

        if (quickAttack)
        {
            quickAttack = Instantiate(quickAttack);
            quickAttack.transform.parent = transform;
        }

        if (heavyAttack)
        {
            heavyAttack = Instantiate(heavyAttack);
            heavyAttack.transform.parent = transform;
        }

        if(quickAttack && heavyAttack)
            GunnerAdjustment();
    }

    void GunnerAdjustment()
    {
        quickAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
        heavyAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
    }

    public void PlayParticleQuickAttack()
    {
        quickAttack.Play();
    }

    public void PlayParticleHeavyAttack()
    {
        heavyAttack.Play();
    }

    public ParticleSystem GetHitSpark()
    {
        return generalHitSpark;
    }
}
