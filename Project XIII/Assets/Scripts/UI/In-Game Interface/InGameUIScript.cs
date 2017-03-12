using UnityEngine;
using System.Collections;

public class InGameUIScript : MonoBehaviour {

    public GameObject pausePanel;               //Panel made to appear on pause
    public GameObject playerStatusPanel;        //Player status panel made to appear on combat



    void Start()
    {
        reset();
    }

    void Update()
    {
        checkForPause();
    }

    //Resets in-game ui to default state
    void reset()
    {
        pausePanel.SetActive(false);
    }


    /*
     *  ------------------Pause Panel Control
     */ 

    //Checks for pause button and reacts accordingly
    void checkForPause()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Any_Start"))
        {
            if (!pausePanel.activeSelf)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pausePanel.GetComponent<PauseMenu>().Reset();
                pausePanel.SetActive(false);
                Time.timeScale = 1.0f;
            }
        } 
    }

    public void ActivateCombatUI()
    {
        playerStatusPanel.SetActive(true);
    }

    public void DeactivateCombatUI()
    {
        playerStatusPanel.SetActive(false);
    }
}
