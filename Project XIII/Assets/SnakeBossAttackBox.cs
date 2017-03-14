using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBossAttackBox : MonoBehaviour {
    public int damage = 40;
    void OnTriggerEnter2D(Collider2D triggerObject)
    {
        if (triggerObject.tag == "Player")
        {
            triggerObject.GetComponent<PlayerProperties>().TakeDamage(damage);
        }
    }

}
