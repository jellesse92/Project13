using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DialogueControllerScript : MonoBehaviour {

    //---Serializables

    [System.Serializable]
    public class PortraitEntry                                  //For making an dictionary entry for Portraits
    {
        public string name;                                     //Name of the character
        public Sprite sprite;                                   //Portrait sprite of character
    }

    //---UI objects for Dialogue
    public GameObject dialogueUI;                               //UI for dialogue to be activated when dialogue is played
    public GameObject skipUIPanel;                              //For skipping dialogue. Jazz's favorite feature
    public Image leftPortrait;                                  //Left speaking portrait
    public Image rightPortrait;                                 //Right speaking portrait
    public GameObject leftNameTag;                              //Name tag of left speaking character
    public GameObject rightNameTag;                             //Name tag of right speaking character
    public GameObject proceedArrow;                             //Arrow to indicate end of current dialogue line
    public Text dialogueUIText;                                 //Typed text for dialogue

    //Other utility scripts
    MusicManager musicManager;                                  //Script that manages music
    SequenceFlowController sequenceScript;                      //Controls flow of the scene

    //---Colors for large portraits
    static Color SPEAKING_COLOR = new Color(1f, 1f, 1f);        //Brights character portrait when speaking
    static Color FADE_COLOR = new Color(.5f, .5f, .5f);         //Darkens character when they are not speaking

    //---Dialogue Text and Portraits
    public TextAsset[] dialogueText;                            //Array of text assets to be used
    public PortraitEntry[] portraitEntries;                     //Array of portrait entries to be loaded into dictionary

    //---Dictionaries
    private Dictionary<string, PortraitEntry> portraitDict;     //Dictionary of sprite portraits

    //---Private variables for controlling dialogue display
    private int currentLine;                                    //Current line being read
    private int currentAsset;                                   //Current text asset being used
    private string[] dialogueArray;                             //Array storing dialogue to be read
    private int textShown;                                      //Amount of text of current line shown
    private bool isTyping;                                      //Determines if current dialogue still being typed
    private bool autoType;                                      //Typing is set to automatically proceed

    //---Command keys for Dialogue
    static KeyCode PROCEED_KEY = KeyCode.C;                     //Proceed with dialogue at normal speed
    static KeyCode SKIP_KEY = KeyCode.Return;                   //Bring up skip scene panel

    void Awake()
    {
        Reset();
        ConstructDict();
        musicManager = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
        sequenceScript = GameObject.FindGameObjectWithTag("Sequence").GetComponent<SequenceFlowController>();
    }

    // Use this for initialization
    void Start () {
        //LoadTextAsset(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (dialogueUI.activeSelf)
        {
            WatchForSkipButton();
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

    //Construct dictionaries
    void ConstructDict()
    {
        portraitDict = new Dictionary<string, PortraitEntry>();
        foreach (PortraitEntry entry in portraitEntries)
        {
            portraitDict[entry.name] = entry;
        }
    }

    //Reset dialogue reading and interface
    void Reset()
    {
        Time.timeScale = 1.0f;
        textShown = 0;
        skipUIPanel.SetActive(false);
        proceedArrow.SetActive(false);
        Clear();
    }

    /*
     *  ------ COMMAND FUNCTIONS
     */ 

    //Watches for input to activate or deactivate skip panel
    void WatchForSkipButton()
    {
        if (Input.GetKeyDown(SKIP_KEY) && !skipUIPanel.activeSelf)
        {
            skipUIPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Input.GetKeyDown(SKIP_KEY) && skipUIPanel.activeSelf)
        {
            CancelSkip();
        }
    }

    //Proceeds dialogue along or faster with input
    void WatchForProceedButton()
    {
        if (Input.GetKeyDown(PROCEED_KEY) && !isTyping)
        {
            ProceedDialogue();
        }
        else if (Input.GetKeyDown(PROCEED_KEY) && isTyping)
        {
            textShown = dialogueArray[currentLine].Length;
        }
    }

    /*
     *  ------ END COMMAND FUNCTIONS
     */


    //Cancel skip action
    public void CancelSkip()
    {
        skipUIPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    /*
     *  ------ DIALOGUE READING FUNCTIONS
     */

    //Loads text to be read to current dialogue 
    public void LoadTextAsset(int index)
    {
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

    //Types in dialogue text
    void typeDialogue()
    {
        proceedArrow.SetActive(!isTyping);

        int length = dialogueArray[currentLine].Length;
        textShown += 1;
        textShown = Math.Min(length, textShown);
        dialogueUIText.text = dialogueArray[currentLine].Substring(0, textShown);
        isTyping = !(textShown == length);
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
            img.sprite = portraitDict[key].sprite;
        }
        else
        {
            img.gameObject.SetActive(false);
        }

    }

    //Sets speaker character
    void setCharSpeaker(Image img, GameObject nameTag)
    {
        img.color = SPEAKING_COLOR;
        nameTag.SetActive(true);
    }

    //Sets character to be listener
    void setCharListener(Image img, GameObject nameTag)
    {
        img.color = FADE_COLOR;
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
        sequenceScript.NextSequence();
    }

    /*
     *  ------ DIALOGUE READING FUNCTIONS
     */
}
