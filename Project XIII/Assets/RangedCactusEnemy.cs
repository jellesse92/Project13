﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCactusEnemy : BasicTurretEnemy
{
    Animator myAnim;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    public override void ApproachTarget()
    {
        if (myAnim.enabled == false)
            myAnim.enabled = true;
        base.ApproachTarget();
    }

    protected override void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
}
