using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour
{

    const float RETICLE_SPEED = 2f;        //Speed reticle set to move at

    Vector2 origin;                         //Original position to return to on reset

    public GameObject selectedCharPanel;    //Panel for selected characters
    Animator selectedCharAnim;              //Selected Character Animation

    public int joystick;                    //Joystick to control reticle
    Vector3 moveDir;                        //Direction to move

    string xInputAxis;                      //X-axis input name for player
    string yInputAxis;                      //Y-axis input name for player

    int currentChar;                        //Number representing character currently being examined by reticle
    bool charSelected;                      //Determines if character has already been selected by player

    //For exiting animation after leaving all character selections with reticle
    int lastChar;

    public AudioClip buttonHighlight;

    AudioSource myAudio;
    Camera cam;

    Animator characterPanel;
    Animator lastCharacterPanel;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();

        moveDir = new Vector3();
        selectedCharAnim = selectedCharPanel.GetComponent<Animator>();
        currentChar = 0;
        lastChar = 0;
        origin = transform.position;

        charSelected = false;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (joystick == 0)
            WatchForMouseInput();
        else
        {
            //For poor calibration issues with controllers set to 0f
            float x = (Mathf.Abs(Input.GetAxis(xInputAxis)) > 0.05) ? Input.GetAxis(xInputAxis) : 0f;
            float y = (Mathf.Abs(Input.GetAxis(yInputAxis)) > 0.05) ? Input.GetAxis(yInputAxis) : 0f;

            moveDir = new Vector3(x * RETICLE_SPEED, y * RETICLE_SPEED, 0f);

            transform.position = transform.position + moveDir;
        }





    }

    void FixedUpdate()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f - 10f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        myAudio.clip = buttonHighlight;
        myAudio.Play();

        if (characterPanel != null)
            lastCharacterPanel = characterPanel;
        characterPanel = col.GetComponent<Animator>();

        if (!charSelected)
        {
            if (col.name == "Gunner Portrait")
                ExamineChar("gunnerSelected", 2);
            else if (col.name == "Swordsman Portrait")
                ExamineChar("swordsmanSelected", 1);
            else if (col.name == "Mage Portrait")
                ExamineChar("mageSelected", 3);
            else if (col.name == "Mech Portrait")
                ExamineChar("mechSelected", 4);
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        characterPanel.SetTrigger("deselect");
        if (lastCharacterPanel != null)
            lastCharacterPanel.SetTrigger("deselect");

        if (currentChar == lastChar && !charSelected)
        {
            currentChar = 0;
            selectedCharAnim.SetTrigger("exit");
        }
        else
            lastChar = currentChar;
    }


    //Character last examined or passed over
    void ExamineChar(string animName, int charType)
    {
        characterPanel.SetTrigger("selected");
        selectedCharAnim.SetTrigger(animName);
        currentChar = charType;
    }

    //Retrieve information from reticle about character being currently examined for selection
    public int GetCharExamine()
    {
        return currentChar;
    }

    //Set character as selected and disable animation switching of selected character panel
    public void CharacterSelected()
    {

        selectedCharAnim.SetTrigger("selected");
        charSelected = true;
        gameObject.SetActive(false);
    }

    //Get if character has been selected
    public bool GetSelectedStatus()
    {
        return charSelected;
    }

    //Set character as deselected and reenable animation swithcing of selected character panel
    public void CharacterDeselected()
    {
        charSelected = false;
    }

    public void Leave()
    {
        currentChar = 0;
        charSelected = false;
        selectedCharAnim.SetTrigger("exit");
        lastChar = 0;
        gameObject.SetActive(false);
    }

    //Watch for mouse movement and input if player 1
    void WatchForMouseInput()
    {

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 currentPos = transform.position;
            currentPos.x = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
            currentPos.y = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
            transform.position = Camera.main.ViewportToWorldPoint(currentPos);
        }


    }

    //Joystick which the reticle should be taking input from
    public void SetJoystick(int num)
    {
        joystick = num;

        if(num != 0)
        {
            xInputAxis = num.ToString() + "_LeftJoyStickX";
            yInputAxis = num.ToString() + "_LeftJoyStickY";
        }
    }


}
