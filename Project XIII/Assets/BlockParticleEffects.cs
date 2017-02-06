using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockParticleEffects : ParticleEffects {
    public GameObject gravityRockFragment;
	
	protected override void ChildSpecificAwake() {
        InstantiateParticle(ref gravityRockFragment);
    }
}
