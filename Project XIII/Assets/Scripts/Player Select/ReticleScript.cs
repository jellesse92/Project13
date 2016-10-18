using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour {

    const float RETICLE_SPEED = 10f;        //Speed reticle set to move at

    public GameObject selectedCharPanel;    //Panel for selected characters
    Animator selectedCharAnim;              //Selected Character Animation

    public int player;                      //Player to control reticle
    Vector2 moveDir;                        //Direction to move

    string xInputAxis;                      //X-axis input name for player
    string yInputAxis;                      //Y-axis input name for player

    void Start()
    {
        xInputAxis = player.ToString() + "_LeftJoyStickX";
        yInputAxis = player.ToString() + "_LeftJoyStickY";
        moveDir = new Vector2();
        selectedCharAnim = selectedCharPanel.GetComponent<Animator>();
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
            ExamineChar("gunnerSelected", 0);
        else if (col.name == "Swordsman Portrait")
            ExamineChar("swordsmanSelected", 1);
        else if (col.name == "Mage Portrait")
            ExamineChar("mageSelected", 2);
        else if (col.name == "Mech Portrait")
            ExamineChar("mechSelected", 3);
    }

    //Character last examined or passed over
    void ExamineChar(string animName,int charNum)
    {
        selectedCharAnim.SetTrigger(animName);
        //Do something with second parameter to determine character selected
    }

    //Watch for mouse movement and input if player 1
    void WatchForMouseInput()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            transform.position = Input.mousePosition;
    }


    public void ActivateSelectedPanel()
    {
        //If you want to add an animation to the selected character panel upon joining a game
    }

    //Function to call when player decided to leave game
    public void LeaveSelect()
    {
        selectedCharAnim.SetTrigger("exit");
        //gameObject.SetActive(false);
    }
}
