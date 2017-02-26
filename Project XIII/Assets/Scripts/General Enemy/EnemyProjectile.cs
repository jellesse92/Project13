using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    protected Enemy enemyScript;
    protected int damage = 10;

    public virtual void Start()
    {
        enemyScript = transform.parent.parent.GetComponentInChildren<Enemy>();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Enemy")
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

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public virtual void PlayParticle(bool play){}

    public virtual void Fire(float x, float y){}

    public virtual void Destroy()
    {
        StartCoroutine("DeactivateBullet");
        enemyScript.ReloadAmmo();
    }

    public virtual IEnumerator DeactivateBullet()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        PlayParticle(false);
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(false);
    }
}
