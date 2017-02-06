using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRockFragment : MonoBehaviour {
    /*
    public bool turnForce = true;
    bool pastTurnForce = true;
    bool change = false;

	// Update is called once per frame
	void LateUpdate () {
        if (change)
        {
            TurnForceOverTime(turnForce);
            change = false;
        }
        if (turnForce != pastTurnForce)
        {
            pastTurnForce = turnForce;
            change = true;
        }
    }
    */
    public void TurnForceOverTime(bool turnOn)
    {
        var forceOverLifetime = GetComponent<ParticleSystem>().forceOverLifetime;
        var emmision = GetComponent<ParticleSystem>().emission;
        var collision = GetComponent<ParticleSystem>().collision;
        var rotationOverLifetime = GetComponent<ParticleSystem>().rotationOverLifetime;

        if (turnOn)
        {
            forceOverLifetime.enabled = true;
            emmision.enabled = true;
            collision.enabled = false;
            rotationOverLifetime.enabled = true;
        }
        else
        {
            forceOverLifetime.enabled = false;
            emmision.enabled = false;
            collision.enabled = true;
            rotationOverLifetime.enabled = false;
        }
    }
}
