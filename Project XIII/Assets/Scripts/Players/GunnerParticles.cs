using UnityEngine;
using System.Collections;

public class GunnerParticles : MonoBehaviour {
    public ParticleSystem quickAttack;
    public ParticleSystem heavyAttack;
    public ParticleSystem quickAttackHitSpark;
    //public GameObject heavyHitImpact;

    void Start () {
        quickAttack = Instantiate(quickAttack);
        heavyAttack = Instantiate(heavyAttack);
        quickAttack.transform.parent = transform;
        heavyAttack.transform.parent = transform;

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
        return quickAttackHitSpark;
    }
}
