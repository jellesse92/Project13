using UnityEngine;
using System.Collections;

public class FindDontDestroy : MonoBehaviour {

    public GameObject GameController;
	// Use this for initialization
	void Start () {
        GameController = GameObject.Find("GameController");
	}

    public void SetPlayerCount(int count)
    {
        GameController.GetComponent<GameController>().SetPlayerCount(count);
    }

    public void SetChar(int CharType)
    {
        GameController.GetComponent<GameController>().SetChar(CharType);
    }
}
