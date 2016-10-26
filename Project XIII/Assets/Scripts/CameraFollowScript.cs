using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

    }
}
