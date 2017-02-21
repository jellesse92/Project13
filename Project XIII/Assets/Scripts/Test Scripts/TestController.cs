using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    //SCRIPT IS FOR TESTING

    public bool testingMode = false;                            //Determines if in testing mode

    bool controllerJoined = false;                              //Determines if a controller has taken control of input
    Transform playerList;

	// Use this for initialization
	void Start () {
        if (testingMode && GameObject.FindGameObjectWithTag("PlayerList"))
            playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;
        else
            this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(testingMode)
            if (!controllerJoined)
                WatchForControllerJoin();
	}

    //Forces controller to take over
    void WatchForControllerJoin()
    {
        for (int i = 1; i < 12; i++)
            if (Input.GetButtonDown(i.ToString() + "_X"))
            {
                Debug.Log("TEST CONTROLLER JOIN");
                controllerJoined = true;
                for(int character = 0; character < 4; character++)
                {
                    playerList.GetChild(character).GetComponent<PlayerInput>().SetJoystickNum(i);
                }
            }
    }


}
