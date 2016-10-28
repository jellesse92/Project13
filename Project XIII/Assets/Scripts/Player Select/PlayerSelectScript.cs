using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerSelectScript : MonoBehaviour {

    public GameObject[] selectReticles;             //Reticles to be activated upon player join
    int[] playerJoystick = new int[4];     //Player slot and its assigned joystick


    //Determine if game should be able to start
    int players = 0;                                //Amount of players joined in the game
    int charactersSelected = 0;                     //Determines number of characters selected to check if all players are ready
    int[] selected = new int[4];                    //Determines if player has selected a character
    bool[] charAvailable = new bool[4];             //Determines if character is available for selection
    public AudioClip buttonSelected;

    AudioSource myAudio;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        Cursor.visible = false;
        for (int i = 0; i < 4; i++)
        {
            selected[i] = -1;
            charAvailable[i] = true;
            playerJoystick[i] = -1;
        }

    }

    void Update()
    {
        WatchForPlayerInput();
    }

    void WatchForPlayerInput()
    {
        WatchForPlayerKeyboardInput();
        WatchForCircleButton();
        WatchForXButton();

    }

    //Reacts to "X" button input
    void WatchForXButton()
    {
        for(int i = 1; i < 12; i++)
            if(Input.GetButtonDown(i.ToString() + "_X"))
                ExecuteXFuncs(i);

    }

    //Execute functions based on "x" button input
    void ExecuteXFuncs(int index )
    {
        CheckJoinPlayer(index);
        CheckSelectCharacter(index);
    }

    void WatchForCircleButton()
    {
        for (int i = 1; i < 12; i++)
            if (Input.GetButtonDown(i.ToString() + "_Circle"))
                ExecuteCircleFuncs(i);
    }

    //Execute circle command functions
    void ExecuteCircleFuncs(int index) {

        index = Array.IndexOf(playerJoystick, index);

        if (index <= -1)
            return;

        if (selected[index] > 0 && !selectReticles[index].activeSelf)
            CheckForDeselect(index);
        else if(selectReticles[index].activeSelf)
            CheckQuitPlayer(index);
    }

    //Watches specifically for keyboard to join based on keyboard or controller
    void WatchForPlayerKeyboardInput()
    {
        if (Input.GetMouseButton(0))
            ExecuteXFuncs(0);

        if (Array.IndexOf(playerJoystick, 0) > -1 && (Input.GetKeyDown(KeyCode.Escape)))
            ExecuteCircleFuncs(0);
    }

    //Check if player should join
    void CheckJoinPlayer(int index)
    {

        if (Array.IndexOf(playerJoystick, index) <= -1)
        {
            int next = GetNextAvailablePlayerSlot();

            if (next == -1)
                return;

            selectReticles[next].SetActive(true);
            selectReticles[next].GetComponent<ReticleScript>().SetJoystick(index);
            players++;
            selected[next] = 0;
            playerJoystick[next] = index;
        }

    }

    int GetNextAvailablePlayerSlot()
    {
        for (int i = 0; i < 4; i++)
            if (playerJoystick[i] == -1)
                return i;
        return -1;
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
                charactersSelected++;
                //Play player accept sound and/or animation
                myAudio.clip = buttonSelected;
                myAudio.Play();
                if (charactersSelected == players)
                    PlayerSelectComplete();
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
        selected[index] = -1;
        players--;
        playerJoystick[index] = -1;
        //Play Leave sound?
    }

    //Check for input if player is deselecint a character
    void CheckForDeselect(int index)
    {
        selectReticles[index].SetActive(true);
        ReticleScript rs = selectReticles[index].GetComponent<ReticleScript>();
        int character = rs.GetCharExamine();
        charAvailable[character - 1] = true;
        rs.CharacterDeselected();
        selected[index] = 0;
        charactersSelected--;
        //Play Deselect sound
    }

    //Function to play when all joined players have selected a character
    void PlayerSelectComplete()
    {
        Invoke("LoadNextScene", 5f);
    }

    void LoadNextScene()
    {
        Application.LoadLevel(2);
    }
}
