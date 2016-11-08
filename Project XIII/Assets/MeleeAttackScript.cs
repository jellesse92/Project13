using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackScript : MonoBehaviour {

    float forceX = 0f;
    float forceY = 0f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(20,0f);
        }
    }
    
    public void SetXForce(float xForce)
    {
        forceX = xForce;
    }

    public void SetYForce(float yForce)
    {
        forceY = yForce;
    }
}
