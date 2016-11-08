using UnityEngine;
using System.Collections;

public class GunnerParticles : MonoBehaviour {
    public GameObject quickAttack;
    public GameObject heavyAttack;

    void Start () {
        quickAttack = Instantiate(quickAttack);
        quickAttack.transform.parent = transform;
        quickAttack.transform.position = new Vector3(1.5f, 1, quickAttack.transform.position.z);

        heavyAttack = Instantiate(heavyAttack);
        heavyAttack.transform.parent = transform;
    }

    public void PlayParticleQuickAttack()
    {
        quickAttack.GetComponent<ParticleSystem>().Play();
    }

    public void PlayParticleHeavyAttack()
    {
        heavyAttack.GetComponent<ParticleSystem>().Play();
    }
}
