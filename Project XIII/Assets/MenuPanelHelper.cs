using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void UpdateMusicVolume()
    {
        Slider slider = gameObject.GetComponentsInChildren<Slider>()[0];
        gameObject.GetComponentInParent<PauseMenu>().changeSound((int)(slider.value));
    }
	// Update is called once per frame
	void Update () {
		
	}
}
