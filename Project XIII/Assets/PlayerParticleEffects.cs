using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour {
    public ParticleSystem hitSpark; //character specific particle effect at enemy hit contact

    public ParticleSystem GetHitSpark()
    {
        return hitSpark;
    }

}
