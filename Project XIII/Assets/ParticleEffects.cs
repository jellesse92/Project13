using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour {
    string PARTICLEHOLDERNAME = "Particles";
    GameObject particlesHolder;

    void Awake()
    {
        MakeParticleHolder();
        ChildSpecificAwake();
    }

    protected virtual void ChildSpecificAwake()
    {
        //Use this to add anything for the awake of children
    }
    
    void MakeParticleHolder()
    {
        particlesHolder = new GameObject();
        particlesHolder.name = PARTICLEHOLDERNAME;
        particlesHolder.transform.parent = transform;
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

    protected void ChangeParticlePosition(ref GameObject particle, Vector3 newPosition)
    {
        if (particle)
            particle.transform.position = newPosition;
    }

    public void PlayParticle(GameObject particle)
    {
        if (particle)
            particle.GetComponent<ParticleSystem>().Play();
    }
}
