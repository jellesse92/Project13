using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour {

    const float RETICLE_SPEED = 10f;        //Speed reticle set to move at

    Vector2 origin;                         //Original position to return to on reset

    public GameObject selectedCharPanel;    //Panel for selected characters
    Animator selectedCharAnim;              //Selected Character Animation

    public int player;                      //Player to control reticle
    Vector2 moveDir;                        //Direction to move

    string xInputAxis;                      //X-axis input name for player
    string yInputAxis;                      //Y-axis input name for player

    int currentChar;                        //Number representing character currently being examined by reticle

    void Start()
    {
        xInputAxis = player.ToString() + "_LeftJoyStickX";
        yInputAxis = player.ToString() + "_LeftJoyStickY";
        moveDir = new Vector2();
        selectedCharAnim = selectedCharPanel.GetComponent<Animator>();
        currentChar = 0;
        origin = transform.position;
    }

    void Update()
    {
        if (player == 1)
            WatchForMouseInput();

        //For poor calibration issues with controllers set to 0f
        float x = (Mathf.Abs(Input.GetAxis(xInputAxis)) > 0.05) ? Input.GetAxis(xInputAxis) : 0f; 
        float y = (Mathf.Abs(Input.GetAxis(yInputAxis)) > 0.05) ? Input.GetAxis(yInputAxis) : 0f;

        moveDir = new Vector2(x * RETICLE_SPEED, y * RETICLE_SPEED);
        transform.position = new Vector2(transform.position.x,transform.position.y) + moveDir;

    }

    void FixedUpdate()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f - 10f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Gunner Portrait")
            ExamineChar("gunnerSelected", 1);
        else if (col.name == "Swordsman Portrait")
            ExamineChar("swordsmanSelected", 2);
        else if (col.name == "Mage Portrait")
            ExamineChar("mageSelected", 3);
        else if (col.name == "Mech Portrait")
            ExamineChar("mechSelected", 4);
    }

    //Character last examined or passed over
    void ExamineChar(string animName, int charType)
    {
        selectedCharAnim.SetTrigger(animName);
        currentChar = charType;
    }

    //Retrieve information from reticle about character being currently examined for selection
    public int GetCharExamine()
    {
        return currentChar;
    }

    public void Leave()
    {
        currentChar = 0;
        transform.position = origin;
        selectedCharAnim.SetTrigger("exit");
        gameObject.SetActive(false);
    }

    //Watch for mouse movement and input if player 1
    void WatchForMouseInput()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            transform.position = Input.mousePosition;
    }


}
