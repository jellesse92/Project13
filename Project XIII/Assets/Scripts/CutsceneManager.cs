using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {

    const float RUN_SPEED = .20f;                               //Speed player moves if set to run as action
    const float BORDER_FILL_AMT = .01f;                         //Amount border fills per invoke
    const float BORDER_INVOKE_RATE = .01f;                      //Rate at which invoke is called

    public enum Character {Swordsman,Gunner,Mage,Mech}          //Selectable characters to animate
    public enum Action {Run,Attack,SetPos,None}                 //Actions characters can make during cutscene

    [System.Serializable]
    public class CharacterAction
    {
        public Character character = Character.Swordsman;       //Character to perform action
        public Action action = Action.None;                     //Action character performs
        public Transform destination;                           //Destination to move character based on selected action
    }

    [System.Serializable]
    public class ActionEntry
    {
        public CharacterAction[] characterActions;              //Array of actions characters should make for current action sequence
        public UnityEvent function;                             //Function to possibly be called during action event
    }

    [System.Serializable]
    public class ActionSequence
    {
        public ActionEntry[] actionEntries;                     //List of actions to happen under cutscene
    }

    public Image topCutsceneBorder;                             //Top border for cutscene
    public Image botCutsceneBorder;                             //Bottom border for cutscene               

    Transform playersManager;                                   //For managing player input
    GameObject[] characterList = new GameObject[4];             //List of characters to be controlled by cutscene controller

    public bool playOnAwake = false;                            //Determines if first cutscene should be played as soon as scene starts
    public ActionSequence[] cutscene;

    //Variables for movement
    bool[] isMoving = new bool[4];
    Vector2[] endPoint = new Vector2[4];

    //Variables for managing flow of cutscene
    int currentCutscene = 0;                                    //Cutscene to be played on next cutscene call
    int currentAction = 0;                                      //Current action to be played under current cutscene
    bool currentActionComplete = true;
    bool borderTransitionComplete = false;

    private void Awake()
    {
        playersManager = GameObject.FindGameObjectWithTag("PlayerList").transform;

        for(int i = 0; i < 4; i++)
            characterList[i] = playersManager.GetChild(i).gameObject;

        for (int i = 0; i < 4; i++)
        {
            isMoving[i] = false;
        }

        if (playOnAwake)
            ActivateCutscene(0);
        else
            this.enabled = false;
    } 

    private void FixedUpdate()
    {
        bool movingCheckDone = true;

        for (int i = 0; i < 4; i++)
        {
            if (isMoving[i])
            {
                ApplyRun(i);
                movingCheckDone = false;
            }
        }

        if(borderTransitionComplete && movingCheckDone && currentActionComplete)
        {
            PlayActionSequence();
        }
    }

    public void ActivateCutscene(int index)
    {
        Reset();
        playersManager.GetComponent<PlayerInputManager>().SetInputsActive(false);
        InvokeRepeating("TransitionInBorders", 0f, BORDER_INVOKE_RATE);
    }

    private void Reset()
    {
        currentActionComplete = true;
        for (int i = 0; i < 4; i++)
            isMoving[i] = false;
    } 

    public void EndCutscene()
    {
        this.enabled = false;
        InvokeRepeating("TransitionOutBorders", 0f, BORDER_INVOKE_RATE);
        playersManager.GetComponent<PlayerInputManager>().SetInputsActive(true);
        currentCutscene++;
    }

    void TransitionInBorders()
    {
        if(topCutsceneBorder.fillAmount >= 1f)
        {
            CancelInvoke("TransitionInBorders");
            borderTransitionComplete = true;
            this.enabled = true;
            return;
        }

        topCutsceneBorder.fillAmount += BORDER_FILL_AMT;
        botCutsceneBorder.fillAmount += BORDER_FILL_AMT;
    }

    void TransitionOutBorders()
    {
        if(topCutsceneBorder.fillAmount <= 0f)
        {
            CancelInvoke("TransitionOutBorders");
            return;
        }
        topCutsceneBorder.fillAmount -= BORDER_FILL_AMT;
        botCutsceneBorder.fillAmount -= BORDER_FILL_AMT;

    }

    void PlayActionSequence()
    {
        if(currentAction >= cutscene[currentCutscene].actionEntries.Length)
        {
            EndCutscene();
            return;
        }

        currentActionComplete = false;
        PlayActions(cutscene[currentCutscene].actionEntries[currentAction]);
        currentAction++;
        currentActionComplete = true;
    }

    void PlayCharacterActions(ActionEntry entry)
    {
        foreach(CharacterAction charAction in entry.characterActions)
        {
            switch (charAction.action)
            {
                case Action.Attack: ActionAttack(charAction.character); break;
                case Action.Run: ActionRun(charAction.character, charAction.destination); break;
                case Action.SetPos: ActionSetPos(charAction.character, charAction.destination); break;
                default: break;
            }
        }
    }

    int GetCharEnumInt(Character c)
    {
        switch (c)
        {
            case Character.Swordsman: return 0;
            case Character.Gunner: return 1;
            case Character.Mage: return 2;
            case Character.Mech: return 3;
        }
        return -1;
    }

    void ActionAttack(Character c)
    {
        characterList[GetCharEnumInt(c)].GetComponent<Animator>().SetTrigger("quickAttack");
    }

    void ActionRun(Character c, Transform dest)
    {
        if (dest == null)
            return;

        int index = GetCharEnumInt(c);

        currentActionComplete = false;
        characterList[index].GetComponent<Animator>().SetFloat("speed", 5f);
        isMoving[index] = true;
        endPoint[index] = dest.position;
    }

    void ActionSetPos(Character c, Transform dest)
    {
        characterList[GetCharEnumInt(c)].transform.position = new Vector2(dest.position.x, dest.position.y);
    }

    void PlayActions(ActionEntry entry)
    {
        PlayCharacterActions(entry);
        entry.function.Invoke();
    }

    void ApplyRun(int index)
    {
        Vector2 pos = characterList[index].transform.position;

        if(Mathf.Abs(pos.x - endPoint[index].x) <= RUN_SPEED)
        {
            characterList[index].transform.position = new Vector2(endPoint[index].x, characterList[index].transform.position.y);
            characterList[index].transform.GetComponent<Animator>().SetFloat("speed", 0f);
            isMoving[index] = false;
            return;
        }

        characterList[index].transform.position = Vector2.MoveTowards(characterList[index].transform.position, 
            new Vector2(endPoint[index].x,characterList[index].transform.position.y),RUN_SPEED);
    }

    void AbortSequence()
    {

    }
}
