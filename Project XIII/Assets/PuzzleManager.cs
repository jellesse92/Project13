using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PuzzleManager : MonoBehaviour {
    public UnityEvent actions;
    public float magShakeApperance;
    public float durShakeApperance;
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
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

    public bool executeIfCorrect()
    {
        if (puzzleStateCorrect())
        {
            actions.Invoke();
            mainCamera.GetComponent<CamShakeScript>().StartShake(magShakeApperance, durShakeApperance);
            return true;
        }
        else
            return false;
    }
}
