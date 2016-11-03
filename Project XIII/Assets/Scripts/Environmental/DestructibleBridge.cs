using UnityEngine;
using System.Collections;

public class DestructibleBridge : MonoBehaviour {

    private bool isUpright;
    private Destructible d;
	// Use this for initialization
	void Start () {
        isUpright = true;
        d = gameObject.GetComponent<Destructible>();
    }
	
	// Update is called once per frame
	void Update () {
        if (d.isDestroyed && isUpright)
        {
            bridgeDestroyed();
        }
	}

    private void bridgeDestroyed()
    {
        isUpright = false;
        gameObject.tag = "Ground";
        //Temporary until bridge becomes useable.
        gameObject.layer = 14;
    }
}
