using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour {
    int sceneIndex = 2;

    void Start()
    {
        if (GameData.current != null && GameData.current.isLoaded)
        {
            changePlayersPosition();
            GameData.current.isLoaded = false;
        }
    }

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

    public void LoadGame()
    {
        DataManager.LoadData();
        sceneIndex = GameData.current.scene;
        GameData.current.isLoaded = true;
        ActivateFadeScreenToLoad();
    }

    void changePlayersPosition()
    {
        GameObject[] playersFound = GameObject.FindGameObjectsWithTag("Player");
        Vector3 newPosition = new Vector3();        
        newPosition.x = GameData.current.player1.positionX;
        newPosition.y = GameData.current.player1.positionY;

        foreach (GameObject player in playersFound)
        {
            newPosition.z = player.transform.position.z;
            player.transform.position = newPosition;
        }        
    }
}
