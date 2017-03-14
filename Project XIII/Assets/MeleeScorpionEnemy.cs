using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScorpionEnemy : EnemyPhysics {

    public GameObject meleeAttackBox;

    protected override void EnemySpecificStart()
    {
        anim = transform.GetComponentInChildren<Animator>();
    }

    public override void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    public void ApplyDamage()
    {
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().ApplyDamage();
    }

    public void ResetDamageApply()
    {
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().knockBackForceX = knockBackForceX;
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().knockBackForceY = knockBackForceY;
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().ResetAttackApplied();
    }
}
