using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void UpdateMenuOptions()
    {
        gameObject.GetComponentInParent<PauseMenu>().UpdateDirectionalTitles();
    }

    public void enableInput(int panel)
    {
        gameObject.GetComponentInParent<PauseMenu>().enableInput(panel);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
