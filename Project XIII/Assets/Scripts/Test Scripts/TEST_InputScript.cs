using UnityEngine;
using System.Collections;

public class TEST_InputScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.Z))
            Debug.Log("Z Down");
        if (Input.GetKeyUp(KeyCode.Z))
            Debug.Log("Z released");

        if (Input.GetButtonDown("1_X"))
            Debug.Log("Player 1: X Pressed!");
        if (Input.GetButtonDown("1_Triangle"))
            Debug.Log("Player 1: Triangle Pressed!");
        if (Input.GetButtonDown("1_Square"))
            Debug.Log("Player 1: Square Pressed!");
        if (Input.GetButtonDown("1_Circle"))
            Debug.Log("Player 1: Circle Pressed!");

        if (Input.GetButtonDown("1_Start"))
            Debug.Log("1 start!");
        if (Input.GetButtonDown("2_Start"))
            Debug.Log("2 start!");
        if (Input.GetButtonDown("3_Start"))
            Debug.Log("3 start!");
        if (Input.GetButtonDown("4_Start"))
            Debug.Log("4 start!");
        if (Input.GetButtonDown("5_Start"))
            Debug.Log("5 start!");
        if (Input.GetButtonDown("6_Start"))
            Debug.Log("6 start!");
    }
}
