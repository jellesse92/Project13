using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

    public enum Character {Swordsman,Gunner,Mage,Mech}
    public enum Action {Run,Attack,Fall}

    [System.Serializable]
    public class ActionEntry
    {
        public Character character;
        public Action action;
        public GameObject destination;
    }

    [System.Serializable]
    public class ActionSequence
    {
        public ActionEntry[] actionEntries;
        public bool proceedNext;
    }

    Transform characterList;

    int currentSequence = 0;

    public ActionSequence[] actionSequences;

    private void Awake()
    {
        characterList = GameObject.FindGameObjectWithTag("PlayerList").transform;
    } 

    // Use this for initialization
    void Start () {
        Invoke("PlayActionSequence", 2f);
        //PlayActionSequence();

    }

    private void FixedUpdate()
    {

    }

    public void PlayActionSequence()
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
    }

    GameObject GetChar(ActionEntry act)
    {
        switch (act.character)
        {
            case Character.Swordsman: return characterList.GetChild(0).gameObject;
            case Character.Gunner: return characterList.GetChild(1).gameObject;
            case Character.Mage: return characterList.GetChild(2).gameObject;
            case Character.Mech: return characterList.GetChild(3).gameObject;
        }
        return null;
    }

    void PlayAnim(GameObject targetChar, ActionEntry act)
    {
        switch (act.action)
        {
            case Action.Attack: targetChar.GetComponent<Animator>().SetTrigger("quickAttack"); break;
        }
    }
}
