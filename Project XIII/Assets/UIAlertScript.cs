using UnityEngine;
using System.Collections;

public class UIAlertScript : MonoBehaviour {

    InGameUIScript iguScript;

	// Use this for initialization
	void Start () {
        iguScript = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIScript>();
	}
	
	void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("test");
        if (col.tag == "Enemy")
            iguScript.ActivateCombatUI();
    }
}
