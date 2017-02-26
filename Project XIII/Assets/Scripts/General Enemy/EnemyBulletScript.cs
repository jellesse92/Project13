using UnityEngine;
using System.Collections;

public class EnemyBulletScript : EnemyProjectile {
    string FIREBALL_NAME = "Fireball";
    string IMPACT_NAME = "Impact";

    const float BULLET_SPEED = 10f;

    public AudioClip impactAudio;

    ParticleSystem fireballParticle;
    ParticleSystem impactParticle;
    
    AudioSource myAudio;

    public override void Start()
    {
        base.Start();
        myAudio = GetComponent<AudioSource>();

        if (transform.Find(FIREBALL_NAME))
            fireballParticle = transform.Find(FIREBALL_NAME).GetComponent<ParticleSystem>();
        if (transform.Find(IMPACT_NAME))
            impactParticle = transform.Find(IMPACT_NAME).GetComponent<ParticleSystem>();
    }

    public override void Fire(float x, float y)
    {
        GetComponent<Rigidbody2D>().velocity = -(transform.position - new Vector3(x, y, transform.position.z)).normalized * BULLET_SPEED;
    }

    public void PlayParticle(bool play)
    {
        if (play)
            fireballParticle.Play();
        else
        {
            fireballParticle.Stop();
            impactParticle.Play();
            myAudio.PlayOneShot(impactAudio);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Enemy")
        {
            if (col.tag == "Player")
                col.GetComponent<PlayerProperties>().TakeDamage(damage);
            
            if (!col.GetComponent<Collider2D>().isTrigger)
            {
                StartCoroutine("DeactivateBullet");
                PlayParticle(false);
                enemyScript.ReloadAmmo();
            }

        }
    }

    public void Destroy()
    {
        StartCoroutine("DeactivateBullet");
        PlayParticle(false);
        enemyScript.ReloadAmmo();
    }
 
    IEnumerator DeactivateBullet()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
