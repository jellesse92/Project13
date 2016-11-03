using UnityEngine;
using System.Collections;

public class SequenceFlowController : MonoBehaviour {

    DialogueControllerScript dcScript;

    //TEMPORARY LAYOUT
    int currentSequence = 0;

    //EVERYTHING TEMPORARY FOR THE DEMO

    public GameObject enGroup1;

    void Start()
    {
        dcScript = GameObject.FindGameObjectWithTag("Dialogue Controller").GetComponent<DialogueControllerScript>();
        dcScript.LoadTextAsset(0);
    }


    //THIS IS ESPECIALLY TEMPORARY
    public void NextSequence()
    {
        currentSequence++;
        if (currentSequence == 1)
            enGroup1.GetComponent<FreezeEnemyScript>().UnfreezeEnemies();
    }


}
