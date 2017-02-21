using UnityEngine;
using System.Collections;

public class FightZoneLockScript : MonoBehaviour {

    public GameObject wall;                     //Wall barring player from proceeding
    int unlockRequirement;                      //How many enemies have to die before unlock
    int deadCount;


	// Use this for initialization
	void Start () {
        deadCount = 0;
        unlockRequirement = 0;

        foreach(Transform child in transform)
        {
            if (child.tag == "Enemy")
                unlockRequirement++;
        }

	}
	
    public void ReportDead()
    {
        deadCount++;

        if (deadCount >= unlockRequirement)
        {
            wall.SetActive(false);

        }

    }
}
