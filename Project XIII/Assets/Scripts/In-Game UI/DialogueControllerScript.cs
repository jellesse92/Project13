using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueControllerScript : MonoBehaviour {

    //---Serializables

    [System.Serializable]
    public class PortraitEntry                                  //For making an dictionary entry for Portraits
    {
        public string name;
        public Image smallPortrait;
    }

    //---UI objects for Dialogue
    public GameObject dialogueUI;                               //UI for dialogue to be activated when dialogue is played
    public GameObject skipUIPanel;                              //For skipping dialogue. Jazz's favorite feature
    public Image leftPortrait;                                  //Left speaking portrait
    public Image rightPortrait;                                 //Right speaking portrait
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

    //---Command keys for Dialogue
    static KeyCode PROCEED_KEY = KeyCode.C;                     //Proceed with dialogue at normal speed
    static KeyCode SKIP_KEY = KeyCode.Return;                   //Bring up skip scene panel

    void Awake()
    {
        ConstructDict();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ConstructDict()
    {
        portraitDict = new Dictionary<string, PortraitEntry>();
        foreach (PortraitEntry entry in portraitEntries)
        {
            portraitDict[entry.name] = entry;
        }
    }
}
