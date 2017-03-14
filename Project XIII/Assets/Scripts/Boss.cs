﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy {

    Image healthBar;

    public override void Reset() { }
    public override void FixedUpdate(){}
    protected override void StunHandler(float stunMultiplier){}         //Bosses do not get normal stuns
    protected override void ForceHandler(float xForce, float yForce) {} //Bosses do not get pushed by players
    public override void SetPos(float x, float y) {}                    //Bosses do not get moved by players
    public override void PlayDeath() {}                                 //Bosses have unique death sequences
    public override void SpecificStunCancel() { }
    public override void Bounce(float forceY = 15000){}

    private void Start()
    {
    }

    public override void Damage(int damage, float stunMultiplier = 0, float xForce = 0, float yForce = 0)
    {
        base.Damage(damage, stunMultiplier, xForce, yForce);

        if (healthBar != null)
            if (health >= 0f)
                healthBar.fillAmount = ((float)health / (float)fullHealth);
            else
                healthBar.fillAmount = 0f;
    }

    public void Unfreeze()
    {
        frozen = false;
    }

    public void SetHealthBar(Image bar)
    {
        healthBar = bar;
    }
}
