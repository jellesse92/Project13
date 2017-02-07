using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockParticleEffects : ParticleEffects {
    public GameObject gravityRockFragment;
    public GameObject impactRockFragments;

	protected override void ChildSpecificAwake() {
        InstantiateParticle(ref gravityRockFragment);
        InstantiateParticle(ref impactRockFragments);
    }
}
