using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : EnemyPhysics{

    public GameObject meleeAttackBox;

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
