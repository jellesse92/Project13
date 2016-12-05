using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackRange : MonoBehaviour{

    HashSet<GameObject> targets = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && col.gameObject.GetComponent<PlayerProperties>().alive && !targets.Contains(col.gameObject))
        {
            targets.Add(col.gameObject);
            transform.parent.GetComponent<Enemy>().SetAttackInRange(true);
        }
    }

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && targets.Contains(col.gameObject))
        {
            targets.Remove(col.gameObject);
            if (targets.Count <= 0)
                transform.parent.GetComponent<Enemy>().SetAttackInRange(false);
        }
    }

    public void Reset()
    {
        targets = new HashSet<GameObject>();
    }
}
