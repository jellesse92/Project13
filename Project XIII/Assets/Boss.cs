using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {

    public override void Reset() { }
    public override void FixedUpdate(){}
    public override void Damage(int damage, float stunMultiplier = 0, float xForce = 0, float yForce = 0){}
    public override void SetPos(float x, float y) { }
    public override void PlayDeath() { }
    public override void SpecificStunCancel() { }
    public override void Bounce(float forceY = 15000){}
}
