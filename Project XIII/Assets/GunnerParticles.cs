using UnityEngine;
using System.Collections;

public class GunnerParticles : MonoBehaviour {
    public GameObject quickAttack;
    public GameObject heavyAttack;

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
        quickAttack.GetComponent<ParticleSystem>().Play();
    }

    public void PlayParticleHeavyAttack()
    {
        heavyAttack.GetComponent<ParticleSystem>().Play();
    }
}
