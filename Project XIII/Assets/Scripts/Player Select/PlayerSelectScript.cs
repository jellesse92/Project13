using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerSelectScript : MonoBehaviour {

    public GameObject[] selectReticles;             //Reticles to be activated upon player join

    //Determine if game should be able to start
    int players = 0;                                //Amount of players joined in the game
    int charactersSelected = 0;                     //Determines number of characters selected to check if all players are ready
    int[] selected = new int[4];                    //Determines if player has selected a character
    bool[] charAvailable = new bool[4];             //Determines if character is available for selection


    void Start()
    {
        Cursor.visible = false;
        for (int i = 0; i < 4; i++)
        {
            selected[i] = 0;
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
            ExecuteXFuncs(1);
        if (Input.GetButtonDown("3_X"))
            ExecuteXFuncs(2);
        if (Input.GetButtonDown("4_X"))
            ExecuteXFuncs(3);
    }

    //Execute functions based on "x" button input
    void ExecuteXFuncs(int index )
    {
        CheckJoinPlayer(index);
        CheckSelectCharacter(index);
    }

    void WatchForCircleButton()
    {
        if (Input.GetButtonDown("2_Circle"))
        {
            if (selectReticles[1].activeSelf)
                ExecuteCircleFuncs(1);
        }
        if (Input.GetButtonDown("3_Circle") && selectReticles[2].activeSelf)
        {
            if (selectReticles[2].activeSelf)
                ExecuteCircleFuncs(2);
        }
        if (Input.GetButtonDown("4_Circle") && selectReticles[3].activeSelf)
        {
            if (selectReticles[3].activeSelf)
                ExecuteCircleFuncs(3);
        }
    }

    //Execute circle command functions
    void ExecuteCircleFuncs(int index)
    {
        if (selected[index] == 0)
            CheckQuitPlayer(index);
        else
            CheckForDeselect(index);
    }

    //Watches specifically for player 1 to join based on keyboard or controller
    void WatchForPlayer1Input()
    {
        if ((Input.GetMouseButton(0) || Input.GetButtonDown("1_X"))){
            ExecuteXFuncs(0);
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("1_Circle")))
        {
            if (selectReticles[0].activeSelf)
            {
                ExecuteCircleFuncs(0);
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
            //Play join sound?
        }
    }

    //Check if player should be able to select chosen character
    void CheckSelectCharacter(int index)
    {
        int character = selectReticles[index].GetComponent<ReticleScript>().GetCharExamine();

        if (character <= 0)
            return;

        if (selectReticles[index].activeSelf)
        {
            if (charAvailable[character-1])
            {
                selectReticles[index].GetComponent<ReticleScript>().CharacterSelected();
                charAvailable[character-1] = false;
                selected[index] = character;
                //Play player accept sound and/or animation
            }
            else
            {
                //Play rejection sound?
            }

        }
    }

    //Check for input if player is leaving game
    void CheckQuitPlayer(int index)
    {
        selectReticles[index].GetComponent<ReticleScript>().Leave();
        players--;
        //Play Leave sound?
    }

    void CheckForDeselect(int index)
    {
        ReticleScript rs = selectReticles[index].GetComponent<ReticleScript>();
        int character = rs.GetCharExamine();
        charAvailable[character - 1] = true;
        rs.CharacterDeselected();
        selected[index] = 0;
        //Play Deselect sound
    }

}
