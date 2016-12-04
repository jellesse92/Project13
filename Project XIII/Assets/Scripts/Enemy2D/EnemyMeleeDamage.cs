﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMeleeDamage : MonoBehaviour {

    public float knockBackForceX = 10f;
    public float knockBackForceY = 10f;
    public float stunDuration = .1f;
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
                float xdir = gameObject.transform.parent.localPosition.x;
                float ydir = gameObject.transform.parent.localPosition.y;
                playersAttacked.Add(target);
                target.GetComponent<PlayerProperties>().TakeDamage(transform.parent.GetComponent<Enemy>().attackPower,knockBackForceX*xdir, knockBackForceY *ydir,stunDuration);
            }

        }
    }


    //Reset list keeping track of players already damaged by finished attack
    public void ResetAttackApplied()
    {
        playersAttacked = new HashSet<GameObject>();
    }

}
