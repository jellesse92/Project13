using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageBlizzardScript : MonoBehaviour {

    const float BLIZZARD_STUN_DURATION = 5f;
    const float APPLY_DAMAGE_RATE = 1.5f;

    public GameObject blizzardParticle;

    HashSet<GameObject> enemy = new HashSet<GameObject>();

    GameObject master;
    int damage = 10;

    void Awake()
    {
        GetComponent<Collider2D>().enabled = false;
        Reset();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if (!enemy.Contains(col.gameObject))
            {
                enemy.Add(col.gameObject);
                col.gameObject.GetComponent<Enemy>().StartCoroutine(col.gameObject.GetComponent<Enemy>().ApplyDebuffFreeze(BLIZZARD_STUN_DURATION));
                col.gameObject.GetComponent<Enemy>().Damage(damage, BLIZZARD_STUN_DURATION);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Enemy" && enemy.Contains(col.gameObject))
            enemy.Remove(col.gameObject);
    }

    public void ApplyFrozenEffect()
    {
        foreach (GameObject target in enemy)
        {
            target.GetComponent<Enemy>().StartCoroutine(target.GetComponent<Enemy>().ApplyDebuffFreeze(BLIZZARD_STUN_DURATION));
            target.GetComponent<Enemy>().Damage(damage, BLIZZARD_STUN_DURATION);
        }

    }

    public void SetMaster(GameObject obj)
    {
        master = obj;
        damage = obj.GetComponent<PlayerProperties>().GetPlayerStats().heavyAirAttackStrengh;
    }

    public void ActivateAttack()
    {
        GetComponent<Collider2D>().enabled = true;
        blizzardParticle.GetComponent<ParticleSystem>().Play();

        foreach (Transform child in blizzardParticle.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
        }
        InvokeRepeating("ApplyFrozenEffect", 0f, APPLY_DAMAGE_RATE);
    }

    public void Reset()
    {
        GetComponent<Collider2D>().enabled = false;
        CancelInvoke("ApplyDamageEffect");
        enemy = new HashSet<GameObject>();
        blizzardParticle.GetComponent<ParticleSystem>().Stop();
        foreach (Transform child in blizzardParticle.transform)
        {
            child.GetComponent<ParticleSystem>().Stop();
            Debug.Log("testing");
        }


    }
}
