using UnityEngine;
using System.Collections;

public class AfterImageParticleRotation : MonoBehaviour {

    public Transform character;
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 scale = transform.localScale;
        scale.x = character.localScale.x;

        transform.localScale = scale;
    }
}
