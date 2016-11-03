using UnityEngine;
using System.Collections;

public class FightZoneLockScript : MonoBehaviour {

    public GameObject wall;                     //Wall barring player from proceeding
    int unlockRequirement;                      //How many enemies have to die before unlock
    int deadCount;

	// Use this for initialization
	void Start () {
        deadCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReportDead()
    {
        deadCount++;
    }
}
