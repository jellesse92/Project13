using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerEnemy : EnemyPhysics {

    const float ROTATION_SPEED = -40f;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Activation Field")
        {
            this.enabled = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activation Field")
            this.enabled = false;
    }

    void Update()
    {
        transform.Rotate(0, 0f, ROTATION_SPEED * Time.deltaTime);
    }
}
