using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenScript : MonoBehaviour {

    const float FILL_SCREEN_AMOUNT = .01f;

    GameObject leftScreenWipe;
    GameObject rightScreenWipe;
    GameObject playerStatusUi;

    bool deathTriggered;

	// Use this for initialization
	void Start () {
        leftScreenWipe = transform.GetChild(0).gameObject;
        rightScreenWipe = transform.GetChild(1).gameObject;
	}

    private void Update()
    {
        if (deathTriggered)
        {
            FillScreen(leftScreenWipe);
            FillScreen(rightScreenWipe);
        }
    }

    public void TriggerDeath()
    {
        if (playerStatusUi != null)
            playerStatusUi.SetActive(false);
        deathTriggered = true;
    }

    public void SetPlayerStatusUI(GameObject panel)
    {
        playerStatusUi = panel;
    }

    void FillScreen(GameObject screenWipe)
    {
        if (screenWipe.GetComponent<Image>().fillAmount < 1f)
        {
            screenWipe.GetComponent<Image>().fillAmount += FILL_SCREEN_AMOUNT;
        }
    }
}
