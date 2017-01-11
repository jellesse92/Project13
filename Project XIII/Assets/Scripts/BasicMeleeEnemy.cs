using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : EnemyPhysics{

    public GameObject meleeAttackBox;

    protected override void EnemySpecificStart()
    {
        Vector3 newCenter = transform.position; //adjust center position of the enemy, use to place particles
        newCenter.x -= 2f;
        newCenter.y += 1.5f;
        ChangeCenter(newCenter);
    }

    void ApplyDamage()
    {
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().ApplyDamage();
    }

    void ResetDamageApply()
    {
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().knockBackForceX = knockBackForceX * transform.parent.localScale.x;
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().knockBackForceY = knockBackForceY;
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().ResetAttackApplied();
    }
}
