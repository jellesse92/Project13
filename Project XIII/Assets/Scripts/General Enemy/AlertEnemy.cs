using UnityEngine;
using System.Collections;

public class AlertEnemy : Enemy {

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Detection Field")
        { 
            SetPursuitState(true);
            SetTarget(col.transform.parent.gameObject);
            transform.FindChild("Enemy Alert Field").GetComponent<AlertFieldScript>().AlertAllies();
        }
    }
}
