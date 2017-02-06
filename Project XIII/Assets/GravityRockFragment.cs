using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRockFragment : MonoBehaviour {
    public bool turnForce = true;
    bool pastTurnForce = true;
    bool change = false;

	// Use this for initialization
	void Start () {
		
	}
	
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

    public void TurnForceOverTime(bool turnOn)
    {
        var fo = GetComponent<ParticleSystem>().forceOverLifetime;
        if (turnOn)
            fo.enabled = true;
        else
            fo.enabled = false;
    }
}
