using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackScript : MonoBehaviour {

    public float forceX = 0f;
    public float forceY = 0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().quickAirAttackStrength, .1f);

            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX * transform.parent.localScale.x, forceY));
        }
    }
    

}
