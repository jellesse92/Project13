using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackScript : MonoBehaviour {

    public float forceX = 0f;
    public float forceY = 0f;
    public int damageMultiplier = 4;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX * transform.parent.localScale.x, forceY));
            col.gameObject.GetComponent<Enemy>().Damage(damageMultiplier * transform.parent.GetComponent<PlayerProperties>().GetPhysicStats().quickAttackStrength, 0f);
        }
    }
    

}
