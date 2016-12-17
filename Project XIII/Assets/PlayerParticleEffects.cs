using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour {
    //All public variables uses prefab, no need to put objects in scene
    public GameObject quickAttack;
    public GameObject heavyAttack;
    public GameObject quickAttackHitSpark;
    public GameObject generalHitSpark;
    //public GameObject heavyHitImpact;

    void Awake() //Awake since other scripts will need the variables here at start
    {
        InstantiateParticles();
    }

    void InstantiateParticles()
    {
        //Tried putting all three into a function, cannot put into parameter of function. 
        //InstantiateParticle(generalHitSpark), InstantiateParticle(quickAttack), InstantiateParticle(heavyAttack)
        //It loses reference. must be done directly. Maybe there's another way?

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

        if (quickAttack && heavyAttack)
            GunnerAdjustment();
    }

    void GunnerAdjustment()
    {
        quickAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
        heavyAttack.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + 1, transform.position.z);
    }

    public void PlayParticleQuickAttack()
    {
        quickAttack.GetComponent<ParticleSystem>().Play();
    }

    public void PlayParticleHeavyAttack()
    {
        heavyAttack.GetComponent<ParticleSystem>().Play();
    }

    public ParticleSystem GetHitSpark()
    {
        return generalHitSpark.GetComponent<ParticleSystem>();
    }
}
