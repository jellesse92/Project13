using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {
    string FIREBALL_NAME = "Fireball";
    string IMPACT_NAME = "Impact";

    int damage = 10;
    ParticleSystem fireballParticle;
    ParticleSystem impactParticle;

    void Start()
    {
        if (transform.Find(FIREBALL_NAME))
            fireballParticle = transform.Find(FIREBALL_NAME).GetComponent<ParticleSystem>();
        if (transform.Find(IMPACT_NAME))
            impactParticle = transform.Find(IMPACT_NAME).GetComponent<ParticleSystem>();
    }

    public void PlayParticle(bool play)
    {
        Debug.Log(fireballParticle);

        if (play)
            fireballParticle.Play();
        else
        {
            fireballParticle.Stop();
            impactParticle.Play();
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
                transform.parent.parent.GetComponent<Enemy>().ReloadAmmo();
            }

        }
    }
    
    IEnumerator DeactivateBullet()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(false);
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
