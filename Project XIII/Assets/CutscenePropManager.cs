using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePropManager : MonoBehaviour {

    CutsceneManager cutsceneScript;

	// Use this for initialization
	void Start () {
        cutsceneScript = GameObject.FindGameObjectWithTag("In Game UI").transform.GetComponentInChildren<CutsceneManager>();
	}

    public CutsceneManager GetScript()
    {
        return cutsceneScript;
    }
}
