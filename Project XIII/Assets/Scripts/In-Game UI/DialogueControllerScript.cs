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
        public Sprite smallPortrait;                            //Small face portrait of character
        public Sprite fullPortrait;                             //Full portrait of character
    }

    //---UI objects for Dialogue
    public GameObject dialogueUI;                               //UI for dialogue to be activated when dialogue is played
    public GameObject skipUIPanel;                              //For skipping dialogue. Jazz's favorite feature
    public GameObject leftPortrait;                             //Left speaking portrait
    public GameObject rightPortrait;                            //Right speaking portrait
    public GameObject proceedArrow;                             //Arrow to indicate end of current dialogue line
    public Image speakingPortrait;                              //Portrait of character currently speaking
    public Text nameTag;                                        //Speaking character name tag
    public Text dialogueUIText;                                 //Typed text for dialogue

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
    }

    // Use this for initialization
    void Start () {
        LoadTextAsset(0);
	}
	
	// Update is called once per frame
	void Update () {
        WatchForSkipButton();
        WatchForProceedButton();
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
        leftPortrait.SetActive(false);
        rightPortrait.SetActive(false);
        skipUIPanel.SetActive(false);
        proceedArrow.SetActive(false);
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

    void ExecuteTxtCommand(string[] command)
    {

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

    //Gives character active color and reveals their nameplate
    void setCharSpeaker(Image portrait, GameObject namePanel)
    {
        portrait.color = SPEAKING_COLOR;
        namePanel.SetActive(true);
    }

    void setCharListener(Image portrait, GameObject namePanel)
    {
        portrait.color = FADE_COLOR;
        namePanel.SetActive(false);
    }

    //Ends current dialogue display
    public void EndDialogue()
    {
        Reset();
        dialogueUI.SetActive(false);
    }

    /*
     *  ------ DIALOGUE READING FUNCTIONS
     */
}
