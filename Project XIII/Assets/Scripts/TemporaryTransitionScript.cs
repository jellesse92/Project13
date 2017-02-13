using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TemporaryTransitionScript : MonoBehaviour {
    public int sceneIndex = 0;
    LoadSceneManager loadSceneManager;
    void Start()
    {
        GameObject loadSceneFound = GameObject.FindWithTag("LoadSceneManager");
        if(loadSceneFound)
            loadSceneManager = loadSceneFound.GetComponent<LoadSceneManager>();
    }
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if (loadSceneManager)
            {
                loadSceneManager.ChangeSceneIndex(sceneIndex);
                loadSceneManager.ActivateFadeScreenToLoad();
            }
            else
                SceneManager.LoadScene(sceneIndex);
        }
    }
}
