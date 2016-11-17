using UnityEngine;
using System.Collections;

public class EnemyGroupController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    foreach(Transform child in transform)
        {
            if (child.tag == "Enemy")
                child.GetComponent<Enemy>().speed += Random.Range(-.06f, .06f);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
