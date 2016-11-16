using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageMeteorScript : MonoBehaviour {

    const float METEOR_STUN_DURATION = .5f;
    const float APPLY_DAMAGE_RATE = .2f;

    public GameObject hitSparkEffect;
    public GameObject impactParticle;
    public GameObject meteorParticle;

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

    void FixedUpdate()
    {
        foreach (GameObject target in enemy)
        {
            target.transform.position = transform.position + new Vector3(0f,-1f,0f);
        }


        if (Physics2D.Raycast(transform.position, -Vector2.up, 1f, LayerMask.GetMask("Default")) &&
            !impactParticle.GetComponent<ParticleSystem>().isPlaying)
        {
            impactParticle.GetComponent<ParticleSystem>().Play(); 
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
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
            transform.parent.parent.GetComponent<PlayerEffectsManager>().ScreenShake(.01f);
        hitSparkEffect.GetComponent<ParticleSystem>().Play();
        foreach (GameObject target in enemy)
            target.GetComponent<Enemy>().Damage(damage, METEOR_STUN_DURATION);
    }

    public void SetMaster(GameObject obj)
    {
        master = obj;
        damage = obj.GetComponent<PlayerProperties>().GetPhysicStats().heavyAirAttackStrengh;
    }

    public void ActivateAttack()
    {
        GetComponent<Collider2D>().enabled = true;
        meteorParticle.GetComponent<ParticleSystem>().Play();

        foreach (Transform child in meteorParticle.transform)
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
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        meteorParticle.GetComponent<ParticleSystem>().Stop();
        impactParticle.GetComponent<ParticleSystem>().Stop();

        foreach(Transform child in impactParticle.transform)
        {
            child.GetComponent<ParticleSystem>().Stop();
            Debug.Log("testing");
        }
        

    }
}
