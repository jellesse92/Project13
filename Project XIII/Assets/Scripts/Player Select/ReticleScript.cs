using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour {

    const float RETICLE_SPEED = 10f;        //Speed reticle set to move at

    public int player;                      //Player to control reticle
    Vector2 moveDir;                        //Direction to move

    string xInputAxis;                      //X-axis input name for player
    string yInputAxis;                      //Y-axis input name for player

    void Start()
    {
        xInputAxis = player.ToString() + "_LeftJoyStickX";
        yInputAxis = player.ToString() + "_LeftJoyStickY";
        moveDir = new Vector2();
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

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
    }

    void WatchForMouseInput()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            transform.position = Input.mousePosition;
    }
}
