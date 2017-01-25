using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PuzzleManager : MonoBehaviour {
    public UnityEvent actions;

    bool puzzleStateCorrect()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Crystal")
                if (!child.GetComponent<CrystalProperties>().isColorCorrect())
                    return false;
        }
        return true;
    }

    public void executeIfCorrect()
    {
        if(puzzleStateCorrect())
            actions.Invoke();
    }
}
