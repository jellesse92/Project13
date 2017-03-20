using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePortraitAnimTracker : MonoBehaviour {

    private Animator myAnim;
    private int currentCharacter = 0;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
    } 

    private void OnEnable()
    {
        if (currentCharacter != 0)
            myAnim.SetInteger("setCharacter", currentCharacter);

    }

    public void SetCurrentChar(int num)
    {
        currentCharacter = num;
    }
}
