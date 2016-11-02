using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMeleeDamage : MonoBehaviour {

    int damage = 10;

    HashSet<GameObject> playersinRange = new HashSet<GameObject>();
    HashSet<GameObject> playersAttacked = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            playersinRange.Add(col.gameObject);
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && playersinRange.Contains(col.gameObject))
        {
            playersinRange.Remove(col.gameObject);
            if (playersAttacked.Contains(col.gameObject))
                playersAttacked.Remove(col.gameObject);
        }
    }

    public void ApplyDamage()
    {
        foreach(GameObject target in playersinRange)
        {
            if (!playersAttacked.Contains(target))
            {
                playersAttacked.Add(target);
                target.GetComponent<PlayerProperties>().TakeDamage(transform.parent.GetComponent<Enemy>().attackPower);
            }

        }
    }

    //Reset list keeping track of players already damaged by finished attack
    public void ResetAttackApplied()
    {
        playersAttacked = new HashSet<GameObject>();
    }

}
