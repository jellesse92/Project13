using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    int selected = 1;
    static string[] MenuOptions = { "Exit", "Stats", "Commands", "Settings" };
    GameObject leftTitle, rightTitle;
    GameController gc;
    AudioSource[] sfxObjects;
    List<float> maxSfxVolumes;
    public bool sfxSound = true;
    Animator anim;

    void Awake()
    {
        selected = 1;
        maxSfxVolumes = new List<float>();
        anim = GetComponentInChildren<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sfxObjects = FindObjectsOfType<AudioSource>();
        leftTitle = GameObject.Find("LeftTitle");
        rightTitle = GameObject.Find("RightTitle");
        UpdateDirectionalTitles();
        foreach (AudioSource sfxObject in sfxObjects)
        {
            maxSfxVolumes.Add(sfxObject.volume);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
        {
            selected = selected > 0 ? selected - 1 : MenuOptions.Length - 1;
            ResetTriggers();
            anim.SetTrigger("Left");

        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
        {
            selected = selected < (MenuOptions.Length - 1) ? selected + 1 : 0; 
            ResetTriggers();
            anim.SetTrigger("Right");
        }
        UpdateDirectionalTitles();
    }
    void IncreaseSound()
    {
        if (gc.IsMusicOn) { }
    }

    void UpdateDirectionalTitles()
    {
        leftTitle.GetComponent<Text>().text = MenuOptions[selected == 0? MenuOptions.Length - 1 : selected - 1];
        rightTitle.GetComponent<Text>().text = MenuOptions[selected == MenuOptions.Length - 1 ? 0 : selected + 1];
    }

    void ResetTriggers()
    {
        anim.ResetTrigger("Left");
        anim.ResetTrigger("Right");
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
