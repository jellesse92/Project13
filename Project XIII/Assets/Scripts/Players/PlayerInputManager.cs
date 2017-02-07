using UnityEngine;
using System.Collections;

public class PlayerInputManager : MonoBehaviour {

    GameController gcScript;

	// Use this for initialization
	void Awake () {
        gcScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gcScript.AssignInputs(transform);
    }

    public void SetInputsActive(bool b)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<PlayerInput>().SetInputActive(b);
        }
    }
 
	
}
