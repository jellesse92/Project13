using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageMeteorScript : MonoBehaviour {


    public GameObject hitSparkEffect;
    public GameObject impactParticle;
    public GameObject meteorParticle;

    HashSet<GameObject> enemy = new HashSet<GameObject>();

    GameObject master;

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
            target.transform.position = transform.position;
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
            target.GetComponent<Enemy>().Damage(transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().heavyAirAttackStrengh, .2f);
    }

    public void SetMaster(GameObject obj)
    {
        master = obj;
    }

    public void ActivateAttack()
    {
        GetComponent<Collider2D>().enabled = true;
        meteorParticle.GetComponent<ParticleSystem>().Play();
    }

    public void Reset()
    {
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
