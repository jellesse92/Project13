using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechRocketBarrage : MonoBehaviour {

    const float BARRAGE_STUN_DURATION = 5f;
    const float APPLY_DAMAGE_RATE = 1.5f;

    public GameObject barrageParticle;

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
                col.gameObject.GetComponent<Enemy>().Damage(damage, BARRAGE_STUN_DURATION);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Enemy" && enemy.Contains(col.gameObject))
            enemy.Remove(col.gameObject);
    }

    public void SetMaster(GameObject obj)
    {
        master = obj;
        damage = obj.GetComponent<PlayerProperties>().GetPhysicStats().heavyAirAttackStrengh;
    }

    public void ActivateAttack()
    {
        GetComponent<Collider2D>().enabled = true;
        barrageParticle.GetComponent<ParticleSystem>().Play();

        foreach (Transform child in barrageParticle.transform)
        {
            child.GetComponent<ParticleSystem>().Play();
        }
        InvokeRepeating("ApplyFrozenEffect", 0f, APPLY_DAMAGE_RATE);
    }

    public void Reset()
    {
        GetComponent<Collider2D>().enabled = false;
        CancelInvoke("ApplyDamageEffec");
        enemy = new HashSet<GameObject>();
        barrageParticle.GetComponent<ParticleSystem>().Stop();
        foreach (Transform child in barrageParticle.transform)
        {
            child.GetComponent<ParticleSystem>().Stop();
            Debug.Log("testing");
        }


    }
}
