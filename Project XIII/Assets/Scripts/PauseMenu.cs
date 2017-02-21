using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    static string[] MenuOptions = { "Exit", "Player Stats", "Commands", "Settings" };
    GameController gc;
    AudioSource[] sfxObjects;
    public bool sfxSound = true;

    void Awake()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sfxObjects = FindObjectsOfType<AudioSource>();
    }

    void Update()
    {
        foreach (AudioSource sfxObject in sfxObjects)
        {
            sfxObject.volume = 0;
        }
    }
    void IncreaseSound()
    {
        if (gc.IsMusicOn) { }
    }

    void DecreaseSound()
    {

    }

    void LoadControls()
    {

    }

    void LoadMainPause()
    {

    }

    void IncreaseSFXVol()
    {

    }
    void ReturnToCharacterSelect()
    {

    }

    void QuitGame()
    {

    }

}
