using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour {

    const float RUN_SPEED = .20f;

    public enum Character {Swordsman,Gunner,Mage,Mech}
    public enum Action {Run,Attack,Fall,None}
    public enum NextActivate {SameTime,After,None}

    [System.Serializable]
    public class CharacterAction
    {
        public Character character = Character.Swordsman;
        public Action action = Action.None;
        public Transform destination;
    }

    [System.Serializable]
    public class ActionEntry
    {
        public CharacterAction[] characterActions;
        public UnityEvent function;
        public NextActivate playNext;
    }

    [System.Serializable]
    public class ActionSequence
    {
        public ActionEntry[] actionEntries;
    }

    Transform playersManager;
    GameObject[] characterList = new GameObject[4];

    int currentSequence = 0;

    public ActionSequence[] cutscene;
    bool[] isMoving = new bool[4];
    Vector2[] endPoint = new Vector2[4];

    private void Awake()
    {
        playersManager = GameObject.FindGameObjectWithTag("PlayerList").transform;

        for(int i = 0; i < 4; i++)
            characterList[i] = playersManager.GetChild(i).gameObject;


        for (int i = 0; i < 4; i++)
            isMoving[i] = false;
    } 

    // Use this for initialization
    void Start () {

        ActivateCutscene(0);
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < 4; i++)
        {
            if (isMoving[i])
                ApplyRun(i);
        }
    }

    public void ActivateCutscene(int index)
    {
        playersManager.GetComponent<PlayerInputManager>().SetInputsActive(false);
        PlayActionSequence();
    }

    void PlayActionSequence()
    {
        foreach(ActionEntry act in cutscene[currentSequence].actionEntries)
        {
            PlayActions(act);
        }
        currentSequence++;
    }

    void PlayCharacterActions(ActionEntry entry)
    {
        foreach(CharacterAction charAction in entry.characterActions)
        {
            switch (charAction.action)
            {
                case Action.Attack: ActionAttack(charAction.character); break;
                case Action.Run: ActionRun(charAction.character, charAction.destination); break;
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

        characterList[index].GetComponent<Animator>().SetFloat("speed", 5f);
        isMoving[index] = true;
        endPoint[index] = dest.position;

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
