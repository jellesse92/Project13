using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {

    const float RUN_SPEED = .20f;                               //Speed player moves if set to run as action
    const float BORDER_FILL_AMT = .01f;                         //Amount border fills per invoke
    const float BORDER_INVOKE_RATE = .01f;                      //Rate at which invoke is called

    public enum Character { Swordsman, Gunner, Mage, Mech }     //Selectable characters to animate
    public enum Action { Run, Attack, SetPos, None,             //Actions characters can make during cutscene 
                         FaceRight, FaceLeft,                   //Turn character                       
                         IntroSetPos, EndRun}                   //Actions character make if they're not an active player  
    public enum CameraOptions
    {
        Free,                                                   //Camera is free to move as it normally would
        None,
        Freeze,                                                 //Camera freezes position
        ForceOrtho,                                             //Forces camera size but with transition
        InstantOrthoSet,                                        //Forcefully set camera size instantly
        CancelForceOrtho,                                       //Cancel forced ortho size
        Travel,                                                 //Travels to destination
    }

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
        public CameraAction[] cameraActions;                    //Actions the camera should perform
        public UnityEvent function;                             //Function to possibly be called during action event
        public float delayNextAction = 0f;                      //Time to delay next action
    }

    [System.Serializable]
    public class ActionSequence
    {
        public ActionEntry[] actionEntries;                     //List of actions to happen under cutscene
        public Transform[] characterEndLocations = new Transform[4]; //0 = Swordsman, 1 = Gunner, 2 = Mage, 3 = Mech
        public UnityEvent endEvents;
        public bool playNext;                                   //Sets to play next cutscene right away
    }

    [System.Serializable]
    public class CameraAction
    {
        public CameraOptions cameraOption = CameraOptions.None;
        public Transform destination;
        public float f_param = 0f;
    }

    [System.Serializable]
    public class CharacterCutsceneStatus
    {
        public bool isMoving = false;                           //Is moving on the scene in some way
        public bool isFalling = false;                          //Is falling
        public bool isActive = false;                           //Is an active player on the scene
        public Vector2 destination = new Vector2();                                  
    }

    public Image topCutsceneBorder;                             //Top border for cutscene
    public Image botCutsceneBorder;                             //Bottom border for cutscene 
    public Animator sceneTranstions;                            //For when skipping scenes 
    public GameObject skipUIPanel;                              //Panel with skip option for cutscene
    GameObject cameraColliders;                                 //Collider container to be disabled for cutscene             

    Transform playersManager;                                   //For managing player input
    GameObject[] characterList = new GameObject[4];             //List of characters to be controlled by cutscene controller

    public bool playOnAwake = false;                            //Determines if first cutscene should be played as soon as scene starts
    public ActionSequence[] cutscene;

    //Variables for controlling camera movement
    MultiplayerCamFollowScript camScript;
    Vector2 camDestination = new Vector2();
    bool cameraMoving = false;

    //Variables for movement

    CharacterCutsceneStatus[] characterStatuses = new CharacterCutsceneStatus[4];

    //Variables for managing flow of cutscene
    int currentCutscene = 0;                                    //Cutscene to be played on next cutscene call
    int currentAction = 0;                                      //Current action to be played under current cutscene
    bool currentActionComplete = true;
    bool borderTransitionComplete = false;
    bool forcedHoldAction = false;                              //Overrides all other bools to stop next action from being played right away
    bool runTurnDelay = false;                                  //Delays actions based on turning delay
    bool dialoguePlaying = false;

    private void Awake()
    {
        playersManager = GameObject.FindGameObjectWithTag("PlayerList").transform;
        cameraColliders = GameObject.FindGameObjectWithTag("Camera Wall");
        camScript = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<MultiplayerCamFollowScript>();

        for(int i = 0; i < 4; i++)
        {
            characterStatuses[i] = new CharacterCutsceneStatus();
        }

        characterStatuses[0].isActive = false;

        for(int i = 0; i < 4; i++)
            characterList[i] = playersManager.GetChild(i).gameObject;

        ActivateCutscene(1);

        if (playOnAwake)
            ActivateCutscene(0);
        else
            this.enabled = false;
    }

    private void Update()
    {
        if (this.enabled)
            WatchForSkipButton();
    } 

    private void FixedUpdate()
    {
        bool movingCheckDone = true;

        for (int i = 0; i < 4; i++)
        {
            if (characterStatuses[i].isMoving)
            {
                ApplyRun(i);
                movingCheckDone = false;
            }          
        }

        if(borderTransitionComplete && movingCheckDone && currentActionComplete
            &&! forcedHoldAction && !dialoguePlaying && !runTurnDelay)
        {
            PlayActionSequence();
        }
    }

    //Watches for input to activate or deactivate skip panel
    void WatchForSkipButton()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !skipUIPanel.activeSelf)
        {
            skipUIPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Return) && skipUIPanel.activeSelf)
        {
            CancelSkip();
        }
    }

    //Cancel skip action
    public void CancelSkip()
    {
        skipUIPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }


    public void ActivateCutscene(int index)
    {
        Reset();
        camScript.ActivateCutsceneMode();
        currentCutscene = index;
        if (cameraColliders != null)
            cameraColliders.SetActive(false);
        playersManager.GetComponent<PlayerInputManager>().SetInputsActive(false);
        for (int i = 0; i < 4; i++)
        {
            characterStatuses[i].isActive = characterList[i].activeSelf;
            if (characterList[i].activeSelf)
            {
                characterList[i].GetComponent<Animator>().SetFloat("speed", 0f);
            }
        }


        InvokeRepeating("TransitionInBorders", 0f, BORDER_INVOKE_RATE);
    }

    private void Reset()
    {
        currentActionComplete = true;
        forcedHoldAction = false;
        runTurnDelay = false;
        for (int i = 0; i < 4; i++)
        {
            characterStatuses[i].isMoving = false;
            characterStatuses[i].isFalling = false;
        }
    } 

    public void SkipCutscene()
    {
        if (!(cutscene[currentCutscene].characterEndLocations.Length < 4))
            for (int i = 0; i < 4; i++)
            {
                characterList[i].SetActive(characterStatuses[i].isActive);
                if (cutscene[currentCutscene].characterEndLocations[i] != null)
                    characterList[i].transform.position = new Vector3(cutscene[currentCutscene].characterEndLocations[i].position.x,
                        cutscene[currentCutscene].characterEndLocations[i].position.y, characterList[i].transform.position.z);
            }

        if (dialoguePlaying)
        {
            dialoguePlaying = false;
            transform.parent.GetComponentInChildren<DialogueControllerScript>().EndDialogue();
        }

        Time.timeScale = 1.0f;
        skipUIPanel.SetActive(false);

        camScript.DeactivateForceOrthographicSize();
        camScript.DeactivateCutsceneMode();

        CancelInvoke("TransitionInBorders");
        CancelInvoke("TransitionOutBorders");

        cutscene[currentCutscene].endEvents.Invoke();
        sceneTranstions.SetTrigger("replay");

        topCutsceneBorder.fillAmount = 0f;
        botCutsceneBorder.fillAmount = 0f;

        EndCutscene();
    }

    public void EndCutscene()
    {
        if (cutscene[currentCutscene].playNext)
        {
            currentCutscene++;
            currentAction = 0;
            ActivateCutscene(currentCutscene);
            return;
        }

        this.enabled = false;
        camScript.DeactivateCutsceneMode();
        if (cameraColliders != null)
            cameraColliders.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (!characterStatuses[i].isActive)
                characterList[i].SetActive(false);
        }
        InvokeRepeating("TransitionOutBorders", 0f, BORDER_INVOKE_RATE);
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
            playersManager.GetComponent<PlayerInputManager>().SetInputsActive(true);
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

    void PlayCharacterActions(CharacterAction[] entry)
    {
        foreach(CharacterAction charAction in entry)
        {
            switch (charAction.action)
            {
                case Action.Attack: ActionAttack(charAction.character); break;
                case Action.Run: ActionRun(charAction.character, charAction.destination); break;
                case Action.SetPos: ActionSetPos(charAction.character, charAction.destination); break;
                case Action.FaceLeft: ActionFace(charAction.character, false); break;
                case Action.FaceRight: ActionFace(charAction.character, true); break;
                case Action.IntroSetPos: ActionIntroSetPos(charAction.character, charAction.destination); break;
                case Action.EndRun:ActionEndRun(charAction.character, charAction.destination); break;
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

        if (characterList[index].transform.position.x > dest.position.x)
        {
            ApplyRunTurnDelay(c, dest, false);
            return;
        }

        else if (characterList[index].transform.position.x < dest.position.x)
        {
            ApplyRunTurnDelay(c, dest, true);
            return;
        }
        characterList[index].GetComponent<Animator>().SetFloat("speed", 5f);
        characterStatuses[index].isMoving = true;
        characterStatuses[index].destination = dest.position;
    }

    void ApplyRunTurnDelay(Character c, Transform dest, bool faceRight)
    {
        ActionFace(c, faceRight);
        StartCoroutine(RunTurnDelay(c, dest));
    }

    IEnumerator RunTurnDelay(Character c, Transform dest)
    {
        runTurnDelay = true;
        yield return new WaitForSeconds(.7f);

        int index = GetCharEnumInt(c);

        characterList[index].GetComponent<Animator>().SetFloat("speed", 5f);
        characterStatuses[index].isMoving = true;
        characterStatuses[index].destination = dest.position;

        runTurnDelay = false;
    }

    void ActionSetPos(Character c, Transform dest)
    {
        int index = GetCharEnumInt(c);
        if (!characterStatuses[index].isActive)
            characterList[index].SetActive(true);
        characterList[index].transform.position = new Vector2(dest.position.x, dest.position.y);
    }

    void ActionFace(Character c, bool faceRight)
    {
        characterList[GetCharEnumInt(c)].GetComponent<PlayerPhysics>().SetFacing(faceRight);
    }

    void ActionIntroSetPos(Character c, Transform dest)
    {
        int index = GetCharEnumInt(c);

        //For characters that aren't activated as they aren't selected
        if (!characterStatuses[index].isActive)
        {
            characterList[index].SetActive(true);
            Debug.Log("test");
            
            ActionSetPos(c, dest);
        }

    }

    void ActionEndRun(Character c, Transform dest)
    {
        //For characters that should leave the scene as they aren't played
        if (!characterStatuses[GetCharEnumInt(c)].isActive)
            ActionRun(c, dest);
    }

    void PlayCameraActions(CameraAction[] entries)
    {
        
        foreach(CameraAction action in entries)
        {
            switch (action.cameraOption)
            {
                case CameraOptions.Free: camScript.DeactivateCutsceneMode(); break;
                case CameraOptions.Freeze: camScript.ActivateCutsceneMode(); break;
                case CameraOptions.ForceOrtho: camScript.ForceOrthographicSize(action.f_param); break;
                case CameraOptions.InstantOrthoSet: camScript.ForceInstantSetOrthographicSize(action.f_param); break;
                case CameraOptions.CancelForceOrtho: camScript.DeactivateForceOrthographicSize(); break;
                case CameraOptions.Travel: ActionCameraTravel(action.destination, action.f_param);break;
                default: break;
            }
        }
    }

    void ActionCameraTravel(Transform destination, float instant_or_not)
    {
        if (instant_or_not == 0f)
            camScript.transform.position = new Vector3(destination.position.x, destination.position.y, camScript.transform.position.z);
        else
            camScript.ForceDestination(destination);
    }

    void PlayActions(ActionEntry entry)
    {
        PlayCharacterActions(entry.characterActions);
        PlayCameraActions(entry.cameraActions);
        entry.function.Invoke();
        if (entry.delayNextAction > 0f)
            DelayAction(entry.delayNextAction);
    }

    void ApplyRun(int index)
    {
        Vector2 pos = characterList[index].transform.position;

        if(Mathf.Abs(pos.x - characterStatuses[index].destination.x) <= RUN_SPEED)
        {
            characterList[index].transform.position = new Vector2(characterStatuses[index].destination.x, characterList[index].transform.position.y);
            characterList[index].transform.GetComponent<Animator>().SetFloat("speed", 0f);
            characterStatuses[index].isMoving = false;
            return;
        }

        characterList[index].transform.position = Vector2.MoveTowards(characterList[index].transform.position, 
            new Vector2(characterStatuses[index].destination.x,characterList[index].transform.position.y),RUN_SPEED);
    }

    void AbortSequence()
    {

    }

    public void DelayAction(float duration)
    {
        CancelInvoke("HoldAction");
        forcedHoldAction = true;
        Invoke("HoldActions", duration);
    }

    void HoldActions()
    {
        forcedHoldAction = false;
    }

    public void ActivateDialogueMode()
    {
        dialoguePlaying = true;
    }

    public void DeactivateDialogueMode()
    {
        dialoguePlaying = false;
    }
}
