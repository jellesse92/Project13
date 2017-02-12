using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour {
    int sceneIndex = 2;

    public void ActivateFadeScreenToLoad()
    {//Use this to fade and animator will load scene
        GetComponent<Animator>().SetTrigger("load");
    }

    public void ChangeSceneIndex(int index)
    {//Use to change scene index
        sceneIndex = index;
    }

	public void LoadScene()
    {//Animator uses this to load the scene
        SceneManager.LoadScene(sceneIndex);
    }
}
