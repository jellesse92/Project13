using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerSelectScript : MonoBehaviour {

    public GameObject[] selectReticles;             //Reticles to be activated upon player join

    //Determine if game should be able to start
    int players = 0;                                //Amount of players joined in the game
    int charactersSelected = 0;                     //Determines number of characters selected to check if all players are ready


    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        WatchForPlayerInput();
    }

    void WatchForPlayerInput()
    {
        WatchForPlayer1Input();
 
        if (Input.GetButtonDown("2_X"))
        {
            if (!selectReticles[1].activeSelf)
                JoinPlayer(1);
        }
        if (Input.GetButtonDown("3_X"))
        {
            if (!selectReticles[2].activeSelf)
                JoinPlayer(2);
        }

        if (Input.GetButtonDown("4_X"))
        {
            if(!selectReticles[3].activeSelf)
                JoinPlayer(3);
        }


    }


    //Watches specifically for player 1 to join based on keyboard or controller
    void WatchForPlayer1Input()
    {
        if ((Input.GetMouseButton(0) || Input.GetButtonDown("1_X")))
        {
            if(!selectReticles[0].activeSelf)
                JoinPlayer(0);
        }
    }


    void JoinPlayer(int index)
    {
        selectReticles[index].SetActive(true);
        players++;
        
    }
}
