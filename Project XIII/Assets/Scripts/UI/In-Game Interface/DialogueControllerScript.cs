using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DialogueControllerScript : MonoBehaviour {

    const float TYPE_DELAY_TIME = .010f;                        //Time by which to delay the speed each character is typed

    //---UI objects for Dialogue
    public GameObject dialogueUI;                               //UI for dialogue to be activated when dialogue is played
    public Image leftPortrait;                                  //Left speaking portrait
    public Image rightPortrait;                                 //Right speaking portrait
    public GameObject leftNameTag;                              //Name tag of left speaking character
    public GameObject rightNameTag;                             //Name tag of right speaking character
    public GameObject proceedArrow;                             //Arrow to indicate end of current dialogue line
    public GameObject skipButton;                               //Button indicating ability to skip
    public Text dialogueUIText;                                 //Typed text for dialogue

    public Material fadeMat;                                    //Used to fade portraits

    //Other utility scripts
    MusicManager musicManager;                                  //Script that manages music
    public CutsceneManager cutsceneManager;                            //Controls the flow of cutscene
    CamShakeScript shakeScript;                                 //Controls camera shake

    //---Colors for large portraits
    static Color SPEAKING_COLOR = new Color(1f, 1f, 1f);        //Brights character portrait when speaking
    static Color FADE_COLOR = new Color(.5f, .5f, .5f);         //Darkens character when they are not speaking

    //---Dialogue Text
    TextAsset[] dialogueText;                                   //Array of text assets to be used. Assigned by text item in level

    //---Private variables for controlling dialogue display
    private int currentLine;                                    //Current line being read
    private int currentAsset;                                   //Current text asset being used
    private string[] dialogueArray;                             //Array storing dialogue to be read
    private int textShown;                                      //Amount of text of current line shown
    private bool isTyping;                                      //Determines if current dialogue still being typed
    private bool autoType;                                      //Typing is set to automatically proceed
    private bool delayType = false;                             //For delaying typing speed

    //---Command keys for Dialogue
    static KeyCode PROCEED_KEY = KeyCode.C;                     //Proceed with dialogue at normal speed

    void Awake()
    {
        Reset();

        GameObject musicObject = GameObject.FindGameObjectWithTag("Music");
        if (musicObject != null)
            musicManager = musicObject.GetComponent<MusicManager>();
        
        shakeScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamShakeScript>();
    }

    private void Start()
    {
        GameObject textHolder = GameObject.FindGameObjectWithTag("Text");

        if (textHolder != null)
            dialogueText = textHolder.GetComponent<TextHolderScript>().textAssets;
    }

    // Update is called once per frame
	void Update () {
        if (dialogueUI.activeSelf)
        {
            WatchForProceedButton();
        }
    }

    void FixedUpdate()
    {
        if (dialogueUI.activeSelf)
        {
            typeDialogue();
        }
    }

    //Reset dialogue reading and interface
    void Reset()
    {
        Time.timeScale = 1.0f;
        leftPortrait.color = Color.white;
        rightPortrait.color = Color.white;
        dialogueUIText.text = "";
        textShown = 0;
        delayType = false;
        proceedArrow.SetActive(false);
        skipButton.SetActive(false);
        Clear();
    }

    /*
     *  ------ COMMAND FUNCTIONS
     */ 

    //Proceeds dialogue along or faster with input
    void WatchForProceedButton()
    {
        if (Time.timeScale == 0f)
            return;
        if ((Input.GetKeyDown(PROCEED_KEY) || Input.GetButtonDown("Any_X")) && !isTyping)
        {
            ProceedDialogue();
        }
        else if ((Input.GetKeyDown(PROCEED_KEY) || Input.GetButtonDown("Any_X")) && isTyping)
        {
            textShown = dialogueArray[currentLine].Length;
        }
    }

    /*
     *  ------ END COMMAND FUNCTIONS
     */


    /*
     *  ------ DIALOGUE READING FUNCTIONS
     */

    //Loads text to be read to current dialogue 
    public void LoadTextAsset(int index)
    {
        cutsceneManager.ActivateDialogueMode();
        dialogueUI.SetActive(true);
        currentAsset = index;
        dialogueArray = dialogueText[index].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        BeginRead();
    }

    //Begin reading text. First line should always be a command or have [] and a blank line underneath it
    void BeginRead()
    {
        currentLine = 0;
        InterpretTxtCommands();
        currentLine = 2;
    }

    void EndTypeDelay()
    {
        delayType = false;
    }

    //Types in dialogue text
    void typeDialogue()
    {
        proceedArrow.SetActive(!isTyping);
        skipButton.SetActive(!isTyping);
        if (!delayType)
        {
            int length = dialogueArray[currentLine].Length;
            textShown += 1;
            textShown = Math.Min(length, textShown);
            dialogueUIText.text = dialogueArray[currentLine].Substring(0, textShown);
            delayType = true;
            Invoke("EndTypeDelay",TYPE_DELAY_TIME);
        }
        isTyping = !(textShown == dialogueArray[currentLine].Length);

    }

    //Execute commands given from currenty examined line of text
    void InterpretTxtCommands()
    {

        string examinedLine = dialogueArray[currentLine];
        if (examinedLine.Length > 2)
        {
            string[] commands = examinedLine.Substring(1, examinedLine.Length - 2).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in commands)
            {
                string[] commandInput = str.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ExecuteTxtCommand(commandInput);
            }
        }
        currentLine++;
    }

    //Execute given text commands for dialogue
    void ExecuteTxtCommand(string[] command)
    {

        switch (command[0])
        {
            //Set portrait images
            case ("LPort"): LoadPortraitImage(leftPortrait, command[1]);
                break;
            case ("RPort"): LoadPortraitImage(rightPortrait, command[1]);
                break;

            //SET NAME TAGS
            case ("LName"):
                leftNameTag.SetActive(true);
                leftNameTag.transform.GetChild(0).GetComponent<Text>().text = command[1];
                if (command[1] == "None")
                    leftNameTag.SetActive(false);
                break;
            case ("RName"):
                rightNameTag.SetActive(true);
                rightNameTag.transform.GetChild(0).GetComponent<Text>().text = command[1];
                if (command[1] == "None")
                    rightNameTag.SetActive(false);
                break;

            //Set speaking state. Fade out color if not speaking. Otherwise brighten/stay normal color if speaking
            case ("LSpeaking"):
                if (command[1] == "T") {
                    setCharSpeaker(leftPortrait, leftNameTag);
                    if (command[1] == "None")
                        leftNameTag.SetActive(false);
                }
                else { setCharListener(leftPortrait, leftNameTag); }
                break;
            case ("RSpeaking"):
                if (command[1] == "T") {
                    setCharSpeaker(rightPortrait, rightNameTag);
                    if (command[1] == "None")
                        rightNameTag.SetActive(false);
                }
                else { setCharListener(rightPortrait, rightNameTag); }
                break;
            case ("Music"):
                if (command[1] == "NextLayer")
                    musicManager.ActivateNextClip();
                break;
            case ("CamShake"):
                float shakeAmt = .01f;
                float.TryParse(command[1], out shakeAmt);
                shakeAmt *= .01f;
                shakeScript.StartShake(shakeAmt);

                break;

            //Clear out character information in dialogue
            case ("Clear"): Clear();
                break;
            default: break;
        }
    }


    void Clear()
    {
        DeactivatePortrait(leftPortrait, leftNameTag);
        DeactivatePortrait(rightPortrait, rightNameTag);
    }

    //Deactivates character portrait image and name tag
    void DeactivatePortrait(Image img, GameObject nameTag)
    {
        img.gameObject.SetActive(false);
        nameTag.SetActive(false);
    }

    //Loads character portrait image
    void LoadPortraitImage(Image img, string key)
    {
        if (key != "None")
        {
            img.gameObject.SetActive(true);
            switch (key.ToLower())
            {
                case "gunner": img.GetComponent<Animator>().SetInteger("setCharacter", 1); break;
                case "swordsman": img.GetComponent<Animator>().SetInteger("setCharacter", 2); break;
                case "mage": img.GetComponent<Animator>().SetInteger("setCharacter", 3); break;
                case "mech": img.GetComponent<Animator>().SetInteger("setCharacter", 4); break;
                case "alice": img.GetComponent<Animator>().SetInteger("setCharacter", 5); break;
                case "ardent": img.GetComponent<Animator>().SetInteger("setCharacter", 6); break;
            }

            img.GetComponent<Animator>().SetTrigger("fadeIn");
        }
        else
        {
            img.gameObject.SetActive(false);
        }

    }

    //Sets speaker character
    void setCharSpeaker(Image img, GameObject nameTag)
    {
        img.material = null;
        nameTag.SetActive(true);
    }

    //Sets character to be listener
    void setCharListener(Image img, GameObject nameTag)
    {
        img.material = fadeMat;
        nameTag.SetActive(false);
    }

    //Proceeds with Dialogue
    void ProceedDialogue()
    {
        textShown = 0;
        currentLine++;
        if (currentLine >= dialogueArray.Length)
        {
            EndDialogue();
            return;
        }
        if (currentLine < dialogueArray.Length)
        {
            if (String.IsNullOrEmpty(dialogueArray[currentLine]))
                ProceedDialogue();
            else if (dialogueArray[currentLine][0] == '[')
            {
                InterpretTxtCommands();
                ProceedDialogue();
            }
        }
    }


    //Ends current dialogue display
    public void EndDialogue()
    {
        Reset();
        dialogueUI.SetActive(false);
        cutsceneManager.DeactivateDialogueMode();
    }

    /*
     *  ------ DIALOGUE READING FUNCTIONS
     */
}
