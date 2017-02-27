using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerEnemy : EnemyPhysics {

    const float ROTATION_SPEED = -40f;

    FlameThrower flamethrowerScript;
    bool flameActivated = false;

    private void Start()
    {
        this.enabled = false;
        flamethrowerScript = transform.GetComponentInChildren<FlameThrower>();
    } 

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Activation Field"))
        {
            this.enabled = true;
            flameActivated = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Activation Field"))
            this.enabled = false;
    }

    void Update()
    {
        if(!stunned && !frozen)
            transform.Rotate(0, 0f, ROTATION_SPEED * Time.deltaTime);
    }

    void DeactivateFlameThrower()
    {
        flamethrowerScript.Deactivate();
        flameActivated = false;
    }

    void ActivateFlameThrower()
    {
        if (!flameActivated)
        {
            flamethrowerScript.Reset();
            flameActivated = true;
        }
    }

    public void Reset()
    {
        flameActivated = true;
        flamethrowerScript.Reset();
    } 
}
