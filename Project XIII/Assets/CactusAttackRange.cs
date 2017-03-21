using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusAttackRange : MonoBehaviour {

    public Enemy enemyScript;
    HashSet<GameObject> targets = new HashSet<GameObject>();

    private void FixedUpdate()
    {
        CheckForDead();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && col.gameObject.GetComponent<PlayerProperties>().alive && !targets.Contains(col.gameObject))
        {
            targets.Add(col.gameObject);
            enemyScript.SetAttackInRange(true);
        }
    }

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && targets.Contains(col.gameObject))
        {
            targets.Remove(col.gameObject);
            if (targets.Count <= 0)
                enemyScript.SetAttackInRange(false);
        }
    }

    void CheckForDead()
    {
        HashSet<GameObject> deadTargets = new HashSet<GameObject>();

        foreach (GameObject player in targets)
        {
            if (!player.GetComponent<PlayerProperties>().alive)
                deadTargets.Add(player);
        }

        foreach (GameObject dead in deadTargets)
        {
            targets.Remove(dead);
        }

        if (targets.Count <= 0)
            enemyScript.SetAttackInRange(false);
    }

    public void Reset()
    {
        targets = new HashSet<GameObject>();
    }
}
