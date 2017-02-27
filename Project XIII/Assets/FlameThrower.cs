using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour {

    const float DAMAGE_APPLY_RATE = .2f;

    Collider2D damageCollider;
    HashSet<GameObject> playerHash = new HashSet<GameObject>();
    bool applyDamageInvoked = false;

    private void Awake()
    {
        damageCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (!playerHash.Contains(collision.gameObject))
            {
                playerHash.Add(collision.gameObject);
                float xdir = collision.transform.position.x - transform.parent.position.x > 0 ? 1 : -1;

                collision.gameObject.GetComponent<PlayerProperties>().TakeDamage(10,xdir * 1000f, 1000f, .1f);
                if (!applyDamageInvoked)
                {
                    Invoke("ApplyDamage", DAMAGE_APPLY_RATE);
                    applyDamageInvoked = true;
                }
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerProperties>().alive)
            if (playerHash.Contains(collision.gameObject))
                playerHash.Remove(collision.gameObject);
    }

    public void Reset()
    {
        CancelApplyDamageInvoke();
        damageCollider.enabled = true;

        //TEMPORARY DELETE WHEN PARTICLE ADDED
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Deactivate()
    {
        //Stop producing particles from here?
        damageCollider.enabled = false;

        //TEMPORARY! DELTE WHEN PARTICLE ADDED
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    void CancelApplyDamageInvoke()
    {
        CancelInvoke("ApplyDamage");
        applyDamageInvoked = false;
    }

    void ApplyDamage()
    {
        if (playerHash.Count <= 0)
            CancelApplyDamageInvoke();

        HashSet<GameObject> deadTargets = new HashSet<GameObject>();

        foreach(GameObject target in playerHash)
            if (target.GetComponent<PlayerProperties>().alive)
                target.GetComponent<PlayerProperties>().TakeDamage(10);
            else
                deadTargets.Add(target.gameObject);

        foreach(GameObject dead in deadTargets)
            playerHash.Remove(dead);
    }
}
