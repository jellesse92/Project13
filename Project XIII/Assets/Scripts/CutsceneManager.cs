using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour {

    const float RUN_SPEED = .35f;

    public enum Character {Swordsman,Gunner,Mage,Mech,Dialogue}
    public enum Action {Run,Attack,Fall,None}
    public enum NextActivate {SameTime,After,None}

    [System.Serializable]
    public class ActionEntry
    {
        public Character character;
        public Action action;
        public Transform destination;
        public UnityEvent function;
        public NextActivate playNext;
    }

    [System.Serializable]
    public class ActionSequence
    {
        public ActionEntry[] actionEntries;
    }

    Transform characterList;

    int currentSequence = 0;

    public ActionSequence[] actionSequences;
    bool[] isMoving = new bool[4];
    Vector2[] endPoint = new Vector2[4];

    private void Awake()
    {
        characterList = GameObject.FindGameObjectWithTag("PlayerList").transform;
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
        characterList.GetComponent<PlayerInputManager>().SetInputsActive(false);
        PlayActionSequence();
    }

    void PlayActionSequence()
    {
        foreach(ActionEntry act in actionSequences[currentSequence].actionEntries)
        {
            PlayAction(act);
        }
        currentSequence++;
    }

    void PlayAction(ActionEntry act)
    {
        GameObject targetChar = GetChar(act);
        PlayAnim(targetChar, act);
        act.function.Invoke();
    }

    GameObject GetChar(ActionEntry act)
    {
        switch (act.character)
        {
            case Character.Swordsman: return characterList.GetChild(0).gameObject;
            case Character.Gunner: return characterList.GetChild(1).gameObject;
            case Character.Mage: return characterList.GetChild(2).gameObject;
            case Character.Mech: return characterList.GetChild(3).gameObject;
            default: break;
        }
        return null;
    }

    void PlayAnim(GameObject targetChar, ActionEntry act)
    {
        switch (act.action)
        {
            case Action.Attack: targetChar.GetComponent<Animator>().SetTrigger("quickAttack"); break;
            case Action.Run: StartRun(targetChar, act, act.destination); break;
            //case Action.Fall: target
        }
    }

    void StartRun(GameObject targetChar, ActionEntry act,Transform destination)
    {
        int index = ConvertToInt(act.character);

        if (index == -1 || destination == null)
            return;

        targetChar.GetComponent<Animator>().SetFloat("speed", 1f);
        isMoving[index] = true;
        endPoint[index] = destination.transform.position;
       
    }

    void ApplyRun(int index)
    {
        Vector2 pos = characterList.GetChild(index).position;

        if(Mathf.Abs(pos.x - endPoint[index].x) <= RUN_SPEED)
        {
            characterList.GetChild(index).position = new Vector2(endPoint[index].x, characterList.GetChild(index).position.y);
            characterList.GetChild(index).GetComponent<Animator>().SetFloat("speed", 0f);
            isMoving[index] = false;
            return;
        }

        characterList.GetChild(index).position = Vector2.MoveTowards(characterList.GetChild(index).position, 
            new Vector2(endPoint[index].x,characterList.GetChild(index).position.y),RUN_SPEED);
    }

    int ConvertToInt(Character c)
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

    void AbortSequence()
    {

    }
}
