using UnityEngine;
using System.Collections;

public class AlertEnemy : Enemy {

	void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(inPursuit);
    }
}
