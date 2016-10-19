using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerSelectScript : MonoBehaviour {

    public GameObject[] selectReticles;             //Reticles to be activated upon player join

    //Determine if game should be able to start
    int players = 0;                                //Amount of players joined in the game
    int charactersSelected = 0;                     //Determines number of characters selected to check if all players are ready
    bool[] selected = new bool[4];                  //Determines if player has selected a character
    bool[] charAvailable = new bool[4];             //Determines if character is available for selection


    void Start()
    {
        Cursor.visible = false;
        for (int i = 0; i < 4; i++)
        {
            selected[i] = false;
            charAvailable[i] = true;
        }

    }

    void Update()
    {
        WatchForPlayerInput();
    }

    void WatchForPlayerInput()
    {
        WatchForPlayer1Input();
        WatchForXButton();
        WatchForCircleButton();

    }

    //Reacts to "X" button input
    void WatchForXButton()
    {
        if (Input.GetButtonDown("2_X"))
        {
            CheckJoinPlayer(1);
        }
        if (Input.GetButtonDown("3_X"))
        {
            CheckJoinPlayer(2);
        }
        if (Input.GetButtonDown("4_X"))
        {
            CheckJoinPlayer(3);
        }
    }

    void WatchForCircleButton()
    {
        if (Input.GetButtonDown("2_Circle"))
        {
            if (selected[1] == false && selectReticles[1].activeSelf)
                QuitPlayer(1);
        }
        if (Input.GetButtonDown("3_Circle") && selectReticles[2].activeSelf)
        {
            if (selected[1] == false)
                QuitPlayer(2);
        }
        if (Input.GetButtonDown("4_Circle") && selectReticles[3].activeSelf)
        {
            if (selected[1] == false)
                QuitPlayer(3);
        }
    }


    //Watches specifically for player 1 to join based on keyboard or controller
    void WatchForPlayer1Input()
    {
        if ((Input.GetMouseButton(0) || Input.GetButtonDown("1_X"))){
            CheckJoinPlayer(0);
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("1_Circle")))
        {
            if (selectReticles[0].activeSelf)
            {
                if (selected[0] == false)
                    QuitPlayer(0);
            }

        }

    }

    //Check if player should join
    void CheckJoinPlayer(int index)
    {
        if (!selectReticles[index].activeSelf)
        {
            selectReticles[index].SetActive(true);
            players++;
        }
    }

    //Check if player should be able to select chosen character


    void QuitPlayer(int index)
    {
        selectReticles[index].GetComponent<ReticleScript>().Leave();
        players--;
    }
}
