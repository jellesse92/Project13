using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemy : EnemyPhysics{

    public GameObject meleeAttackBox;
    
    protected override void EnemySpecificStart()
    {
        Vector3 newCenter = transform.position; //adjust center position of the enemy, use to place particles
        newCenter.x -= 0.2f;
        newCenter.y -= 0.3f;
        ChangeCenter(newCenter);
    }

    void ApplyDamage()
    {
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().ApplyDamage();
    }

    void ResetDamageApply()
    {
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().knockBackForceX = knockBackForceX;
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().knockBackForceY = knockBackForceY;
        meleeAttackBox.GetComponent<EnemyMeleeDamage>().ResetAttackApplied();
    }
}
