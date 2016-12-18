using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour {
    //All public variables uses prefab, no need to put objects in scene
    public GameObject quickAttack;
    public GameObject heavyAttack;
    public GameObject quickAttackHitSpark;
    public GameObject generalHitSpark;

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
        //Tried putting all three into a function, cannot put into parameter of function. 
        //InstantiateParticle(generalHitSpark), InstantiateParticle(quickAttack), InstantiateParticle(heavyAttack)
        //It loses reference. must be done directly. Maybe there's another way?

        particlesHolder = new GameObject();
        particlesHolder.name = "Particles";
        particlesHolder.transform.parent = transform;

        if (generalHitSpark) 
        {
            generalHitSpark = Instantiate(generalHitSpark); 
            generalHitSpark.transform.parent = particlesHolder.transform;
        }
        if (quickAttack)
        {
            quickAttack = Instantiate(quickAttack);
            quickAttack.transform.parent = particlesHolder.transform;
        }
        if (heavyAttack)
        {
            heavyAttack = Instantiate(heavyAttack);
            heavyAttack.transform.parent = particlesHolder.transform;
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

    public ParticleSystem GetHitSpark()
    {
        if(generalHitSpark)
            return generalHitSpark.GetComponent<ParticleSystem>();
        return null;
    }
}
