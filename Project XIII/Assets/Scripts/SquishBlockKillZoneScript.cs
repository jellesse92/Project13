using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishBlockKillZoneScript : MonoBehaviour {

    const int DAMAGE = 1000;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerProperties>().TakeDamage(DAMAGE);
        }
    }
}
