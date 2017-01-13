using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonatingEnemyExplosion : MonoBehaviour {

    HashSet<GameObject> playersinRange = new HashSet<GameObject>();
    bool exploding = false;
    bool interrupted = false;
    int damage = 40;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!exploding)
            StartCoroutine("TriggerExplosion");
        if (col.tag == "Player" && !playersinRange.Contains(col.gameObject))
            playersinRange.Add(col.gameObject);

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && playersinRange.Contains(col.gameObject))
            playersinRange.Remove(col.gameObject);
        
    }

    private void FixedUpdate()
    {
        if (interrupted && !transform.parent.GetComponent<Enemy>().GetStunStatus())
        {
            interrupted = false;
            StartCoroutine("TriggerExplosion");
        }
    }

    void ApplyExplosion()
    {
        foreach(GameObject target in playersinRange)
        {
            if (target.GetComponent<PlayerProperties>().alive)
            {
                float knockX = transform.parent.GetComponent<EnemyPhysics>().knockBackForceX;
                float knockY = transform.parent.GetComponent<EnemyPhysics>().knockBackForceY;
                target.GetComponent<PlayerProperties>().TakeDamage(damage, knockX, knockY);
            }
        }

        transform.parent.GetComponent<Enemy>().Damage(1000);
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator TriggerExplosion()
    {
        exploding = true;
        yield return new WaitForSeconds(transform.parent.GetComponent<DetonatingSkullPhysics>().GetExplosionDelay());
        ApplyExplosion();
    }

    public void CancelExplosion()
    {
        StopCoroutine("TriggerExplosion");
        interrupted = true;
    }

    public void Reset()
    {
        exploding = false;
        interrupted = false;
    } 

    public void SetDamage(int d)
    {
        damage = d;
    }
}
