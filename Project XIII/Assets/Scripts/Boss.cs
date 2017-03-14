using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
    public override void Reset() { }
    public override void FixedUpdate(){}
    protected override void StunHandler(float stunMultiplier){}         //Bosses do not get normal stuns
    protected override void ForceHandler(float xForce, float yForce) {} //Bosses do not get pushed by players
    public override void SetPos(float x, float y) {}                    //Bosses do not get moved by players
    public override void PlayDeath() {}                                 //Bosses have unique death sequences
    public override void SpecificStunCancel() { }
    public override void Bounce(float forceY = 15000){}

    public void Unfreeze()
    {
        frozen = false;
    }
}
