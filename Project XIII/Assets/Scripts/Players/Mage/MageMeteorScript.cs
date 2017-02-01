using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageMeteorScript : MonoBehaviour {

    const float BLIZZARD_STUN_DURATION = .5f;
    const float APPLY_DAMAGE_RATE = .2f;

    public GameObject hitSparkEffect;
    public GameObject blizzardParticle;

    HashSet<GameObject> enemy = new HashSet<GameObject>();

    GameObject master;
    int damage = 10;

    void Awake()
    {
        hitSparkEffect = Instantiate(hitSparkEffect);
        hitSparkEffect.transform.parent = transform.parent;
        GetComponent<Collider2D>().enabled = false;
        Reset();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if (!enemy.Contains(col.gameObject))
                enemy.Add(col.gameObject);
        }
    }

    public void ApplyDamageEffect()
    {
        if (transform.parent.parent != null)
            transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(1f);
        foreach (GameObject target in enemy)
            target.GetComponent<Enemy>().Damage(damage, BLIZZARD_STUN_DURATION);
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
        InvokeRepeating("ApplyDamageEffect", 0f, APPLY_DAMAGE_RATE);
    }

    public void Reset()
    {
        GetComponent<Collider2D>().enabled = false;
        CancelInvoke("ApplyDamageEffec");
        enemy = new HashSet<GameObject>();
        blizzardParticle.GetComponent<ParticleSystem>().Stop();
        foreach(Transform child in blizzardParticle.transform)
        {
            child.GetComponent<ParticleSystem>().Stop();
            Debug.Log("testing");
        }
        

    }
}
