using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour {

    GameController gcScript;

	// Use this for initialization
	void Start () {
        gcScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gcScript.AssignInputs(transform);
    }
	
}
