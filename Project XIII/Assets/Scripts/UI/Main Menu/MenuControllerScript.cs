using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class MenuControllerScript : MonoBehaviour {

    public GameObject PressAnyButton;
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;
    public GameObject HelpPanel;

    GameObject currentPanel;                        //Current panel being viewed
    int currentIndex;                               //Current index of child being viewed

    bool waitForSelect = false;                     //Wait for time to select
    bool newPanel = true;                           //Determines if on new panel

    void Start()
    {
        currentIndex = -1;
        currentPanel = MainMenuPanel;
    }

    void Update()
    {
        if(!CheckAnyButton() && !waitForSelect)
            NavigatePanel();
    }

    void CloseAnim(GameObject animObj)
    {
        animObj.GetComponent<Animator>().SetTrigger("Close");
    }

    void OpenAnim(GameObject animObj)
    {
        animObj.GetComponent<Animator>().SetTrigger("Open");
    }

    bool CheckAnyButton()
    {
        if (!PressAnyButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Press Any Button Open"))
            return false;

        for (int i = 1; i < 12; i++)
        {
            if (Input.GetButtonDown(i.ToString() + "_X") ||
                Input.GetButtonDown(i.ToString() + "_Circle") ||
                Input.GetButtonDown(i.ToString() + "_Triangle") ||
                Input.GetButtonDown(i.ToString() + "_Square") ||
                Input.GetButtonDown(i.ToString() + "_Start"))
            {
                OpenAnim(MainMenuPanel);
                CloseAnim(PressAnyButton);
                SetCurrentPanel(MainMenuPanel);
            }
        }

        return true;
    }

    void NavigatePanel()
    {
        //Debug.Log(Input.GetAxisRaw("Any_LeftJoyStickY"));
        int dir = (int)Input.GetAxisRaw("Any_LeftJoyStickY");
        int minIndex = 0;

        if (dir == 0)
            return;

        if (currentPanel.name == "Setting Panel")
            minIndex = 1;
        else if (currentPanel.name == "Help Panel")
        {
            currentIndex = 2;
            SelectIndex();
            return;
        }


        if (newPanel)
        {
            currentIndex = minIndex;
            newPanel = !newPanel;
        }
        else
            currentIndex += dir;

        if (currentIndex < minIndex)
            currentIndex = currentPanel.transform.childCount - 1;
        if (currentIndex >= currentPanel.transform.childCount)
            currentIndex = minIndex;

        if(currentIndex > -1)
            SelectIndex();

        StartCoroutine("WaitTime");
        
    }

    void SelectIndex()
    {
        currentPanel.transform.GetChild(currentIndex).GetComponent<Button>().Select();
        Debug.Log("Current Index" + currentIndex);
    }

    IEnumerator WaitTime()
    {
        waitForSelect = true;
        yield return new WaitForSeconds(2f);
        waitForSelect = false;
    }

    public void SetCurrentPanel(GameObject panel)
    {
        currentPanel = panel;
        currentIndex = 0;
        newPanel = true;
    }


}
