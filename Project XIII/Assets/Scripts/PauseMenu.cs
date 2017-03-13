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
    public GameObject[] panels;
    public List<RectTransform> panelLocations;
    Animator anim;

    public void Reset()
    {
        print("Reset CalleD");
        ResetTriggers();
        selected = 1;
        leftTitle = GameObject.Find("LeftTitle");
        rightTitle = GameObject.Find("RightTitle");
        UpdateDirectionalTitles();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].GetComponent<RectTransform>().anchoredPosition = panelLocations[i].anchoredPosition;
        }
        SetStats();
    }
    void Awake()
    {
        
        panelLocations = new List<RectTransform>();
        maxSfxVolumes = new List<float>();
        anim = GetComponentInChildren<Animator>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sfxObjects = FindObjectsOfType<AudioSource>();
        foreach (AudioSource sfxObject in sfxObjects)
        {
            maxSfxVolumes.Add(sfxObject.volume);
        }
        foreach( GameObject panel in panels)
        {
            panelLocations.Add(panel.GetComponent<RectTransform>());
        }
        Reset();
        
        

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
    }
    void IncreaseSound()
    {
        if (gc.IsMusicOn) { }
    }

    public void UpdateDirectionalTitles()
    {
        leftTitle.GetComponent<Text>().text = MenuOptions[selected == 0? MenuOptions.Length - 1 : selected - 1];
        rightTitle.GetComponent<Text>().text = MenuOptions[selected == MenuOptions.Length - 1 ? 0 : selected + 1];
    }

    void ResetTriggers()
    {
        anim.ResetTrigger("Left");
        anim.ResetTrigger("Right");
    }

    void SetStats()
    {
        print("hi");
        //int count = panels[1].transform.childCount;
        //print(count);
        //for (int i = 0; i < panels[1].transform.childCount; i++)
        //{
        //    var player = panels[1].transform.GetChild(i);
        //    for(int j = 0; i < player.childCount; i++)
        //    {
        //        //HP = 0, ATTACK = 1, SPEED = 2
        //        player.GetChild(j).GetComponent<Text>().text = "HP: :D";
        //    }
        //}
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
