using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScorpionEnemy : EnemyPhysics {

    protected override void EnemySpecificStart()
    {
        anim = transform.parent.GetComponentInChildren<Animator>();
    }

    public override void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
}
