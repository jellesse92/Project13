using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCToggle : MonoBehaviour {

    [System.Serializable]
    public class NPCPlayer
    {
        public GameObject character;
        public bool enable = false;
    }


    Transform playerList;
    public NPCPlayer[] playerObjects;

	// Use this for initialization
	void Start () {
        playerList = GameObject.FindGameObjectWithTag("PlayerList").transform;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleOn()
    {
        for(int i = 0; i < 4; i++)
        {
            if (playerObjects[i].enable && playerList.GetChild(i).GetComponent<PlayerInput>().GetJoystick() == -1)
                playerObjects[i].character.SetActive(true);
        }
    }
}
