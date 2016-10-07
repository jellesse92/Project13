using UnityEngine;
using System.Collections;

public class AlertEnemy : Enemy {

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Detection Field")
        {
            inPursuit = true;
            target = col.transform.parent.gameObject;
            transform.FindChild("Enemy Alert Field").GetComponent<AlertFieldScript>().AlertAllies();
        }
    }
}
