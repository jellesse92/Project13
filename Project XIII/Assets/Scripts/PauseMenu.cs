using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    bool testing;
    int selected = 1;
    static string[] MenuOptions = { "Exit", "Stats", "Commands", "Settings" };
    GameObject leftTitle, rightTitle;
    GameController gc;
    float currentSFXSound = 100;
    AudioSource[] sfxObjects;
    List<float> maxSfxVolumes;
    public GameObject[] characters;
    public List<PlayerProperties> activePlayers;
    public bool sfxSound = true;
    public GameObject[] panels;
    public List<RectTransform> panelLocations;
    private bool isReadingInput;
    Animator anim;
    KeyConfig inputReader;

    public void Reset()
    {

        isReadingInput = true;
        ResetTriggers();
        selected = 1;
        leftTitle = GameObject.Find("LeftTitle");
        rightTitle = GameObject.Find("RightTitle");
        UpdateDirectionalTitles();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].GetComponent<RectTransform>().anchoredPosition = panelLocations[i].anchoredPosition;
        }
        SetActivePlayers();
        SetStats();
        Slider[] volsliders = panels[3].GetComponentsInChildren<Slider>();
        volsliders[0].value = gc.musicVolume;
        volsliders[1].value = currentSFXSound;

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void UnpauseForSceneChange()
    {
        Reset();
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
    void SetActivePlayers()
    {
        //1 = Swordsman; 2 = Gunner; 3 = Mage; 4 = Mech
        for(int i = 0; i < gc.PlayerCharacters.Length; i++)
        {
            if(gc.PlayerCharacters[i].player >= 1)
            {
                activePlayers.Add(characters[i].GetComponent<PlayerProperties>());
            }
        }
        activePlayers = activePlayers.OrderBy(p => p.playerNumber).ToList();
    }
    void Awake()
    {
        testing = GameObject.FindObjectOfType<TestController>().testingMode;
        inputReader = new KeyConfig();
        activePlayers = new List<PlayerProperties>();
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
        if (isReadingInput)
        {
            bool isInteracting = false;
            if (selected == 3)
            {
                Slider[] options = panels[selected].GetComponentsInChildren<Slider>();
                if ((Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown(inputReader.jumpButton)))
                {
                    foreach (Slider option in options)
                    {
                        option.interactable = !option.IsInteractable();
                    }
                    options[0].Select();
                }
                isInteracting = options[0].IsInteractable();
            }
            if (!isInteracting && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < -0.6))
            {
                isReadingInput = true;
                selected = selected > 0 ? selected - 1 : MenuOptions.Length - 1;
                ResetTriggers();
                anim.SetTrigger("Left");
                EventSystem.current.SetSelectedGameObject(null);

            }
            else if (!isInteracting && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0.6))
            {
                isReadingInput = false;
                selected = selected < (MenuOptions.Length - 1) ? selected + 1 : 0;
                ResetTriggers();
                anim.SetTrigger("Right");
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
    public void changeSound(float amnt)
    {
        if (gc != null)
        {
            gc.ChangeMusicVol(amnt);
        }
    }

    public void changeSFX(float amnt)
    {
        currentSFXSound = amnt;
        for(int i = 0; i < sfxObjects.Length; i++)
        {
            sfxObjects[i].volume = amnt * maxSfxVolumes[i];
        }
    }

    public void UpdateDirectionalTitles()
    {
        leftTitle.GetComponent<Text>().text = MenuOptions[selected == 0? MenuOptions.Length - 1 : selected - 1];
        rightTitle.GetComponent<Text>().text = MenuOptions[selected == MenuOptions.Length - 1 ? 0 : selected + 1];
        if (selected == 0)
        {
            Button[] options = panels[selected].GetComponentsInChildren<Button>();
            options[0].Select();
        }
        
    }

    public void enableInput(int selectedPanel)
    {
        selected = selectedPanel;
        UpdateDirectionalTitles();
        isReadingInput = true;
    }
    void ResetTriggers()
    {
        anim.ResetTrigger("Left");
        anim.ResetTrigger("Right");
    }

    void SetStats()
    {
        Transform playerStatPanel = panels[1].transform.GetChild(1);
        for (int i = 0; i < playerStatPanel.childCount; i++)
        {
            
            var player = playerStatPanel.GetChild(i);
            if (gc.PlayerCount < 2 && i == 1)
            {
                player.gameObject.SetActive(false);
                break;
            }
            if (!testing)
            {
                print("Getting Player Stats");
                var stats = activePlayers[i].GetPlayerStats();
                for (int j = 0; i < player.childCount; j++)
                {
                    if (j == 3) { break; }
                    var title = player.GetChild(j).name;

                    switch (title)
                    {
                        case "HP":
                            player.GetChild(j).GetComponent<Text>().text = "HP: " + activePlayers[i].GetCurrentHealth() + "/" + activePlayers[i].GetMaxHealth();
                            break;
                        case "Attack":
                            player.GetChild(j).GetComponent<Text>().text = "Attack: " + stats.quickAttackStrength + "/" + stats.heavyAttackStrength + " " +
                                stats.quickAirAttackStrength + "/" + stats.heavyAirAttackStrengh;
                            break;
                        case "Speed":
                            player.GetChild(j).GetComponent<Text>().text = "Speed: " + stats.movementSpeed;
                            break;
                    }
                }
            }
        }
    }




}
