using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionAnimatorFunctions : MonoBehaviour {

    MeleeScorpionEnemy enemyScript;

    public ParticleSystem deathParticles;


	// Use this for initialization
	void Start () {
        enemyScript = transform.parent.GetComponent<MeleeScorpionEnemy>();
	}


    void EnableAttackJumpMove()
    {
        enemyScript.EnableAttackJumpMove();
    }

    void DisableAttackJumpMove()
    {
        enemyScript.DisableAttackJumpMove();
    }

    void ResetDamageApply()
    {
        enemyScript.ResetDamageApply();
    }

    void ApplyDamage()
    {
        enemyScript.ApplyDamage();
    }

    void StopDeathParticles()
    {
        deathParticles.loop = false;
    }
}
