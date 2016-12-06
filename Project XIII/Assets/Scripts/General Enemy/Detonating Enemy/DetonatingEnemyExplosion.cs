using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonatingEnemyExplosion : MonoBehaviour {

    HashSet<GameObject> playersinRange = new HashSet<GameObject>();
    bool exploding = false;
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

    void ApplyExplosion()
    {
        foreach(GameObject target in playersinRange)
        {
            
            /*
            if (target.GetComponent<PlayerProperties>().alive)
                target.GetComponent<PlayerProperties>().TakeDamage(damage, knockBackX, knockBackY);
                */
        }
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
    }

    public void Reset()
    {
        exploding = false;
    } 

    public void SetDamage(int d)
    {
        damage = d;
    }
}
