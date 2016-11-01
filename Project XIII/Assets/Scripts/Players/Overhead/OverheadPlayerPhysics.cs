using UnityEngine;
using System.Collections;

public class OverheadPlayerPhysics : MonoBehaviour {
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private KeyPress myKeyPress;
    private PlayerInput myPlayerInput;

    private bool isFacingRight;
    private float movementSpeed = .5f;

    // Use this for initialization
    void Start () {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myPlayerInput = GetComponent<PlayerInput>();
        myKeyPress = myPlayerInput.getKeyPress();
        isFacingRight = true;
    }
	
	// Update is called once per frame
	void Update () {
        myPlayerInput.GetInput();
        myKeyPress = myPlayerInput.getKeyPress();
    }

    void FixedUpdate()
    {
        Movement();

        myPlayerInput.ResetKeyPress();
    }

    void Movement()
    {
        int dir = 0;

        if (Mathf.Abs(myKeyPress.verticalAxisValue) > 0.1f)
        {
            if (myKeyPress.verticalAxisValue > 0f)
                dir = 1;
            else
                dir = -1;
            var move = new Vector3(0, dir, 0);
            transform.position += move * movementSpeed * Time.deltaTime;

            if (!isFacingRight)
                Flip();

            AnimateTopDown(0, dir);
        }
        else if(Mathf.Abs(myKeyPress.horizontalAxisValue) > 0.1f)
        {
            if (myKeyPress.horizontalAxisValue > 0f)
                dir = 1;
            else
                dir = -1;
            var move = new Vector3(dir, 0, 0);

            transform.position += move * movementSpeed * Time.deltaTime;

            if (dir == 1 && !isFacingRight || dir == -1 && isFacingRight)
            {
                Flip();
            }

            AnimateTopDown(1, 0);
        }

        if (dir == 0)
            AnimateTopDown(0, 0);

    }

    void AnimateTopDown(int x, int y)
    {
        myAnimator.SetInteger("velocityX", x);
        myAnimator.SetInteger("velocityY", y);
    }


    void Flip()
    {

        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }
}
