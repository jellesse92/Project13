using UnityEngine;
using System.Collections;

public class PlayerMage : MonoBehaviour {

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    private bool facingRight;
    private bool attack;
    private bool isGrounded;
    private bool isJumping = false;
   
    [SerializeField]
    private float movementSpeed = 10;

    [SerializeField]
    private float jumpForce = 5;

    [SerializeField]
    private float jumpTime;
    
    

    // Use this for initialization
    void Start ()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
	
    void Update()
    {
        HandleInput();
    }
	
	void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            HandleMovement(horizontal);
            Flip(horizontal);
            HandleAttacks();
        }
        else
            myRigidbody.velocity = Vector2.zero;

        ResetValues();

    }

    private void HandleMovement(float horizontal)
    {
        if (isJumping)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        myRigidbody.velocity = new Vector2(horizontal * movementSpeed ,myRigidbody.velocity.y);
        myAnimator.SetFloat("speed",Mathf.Abs(horizontal));
        
    }
    
    private void HandleAttacks()
    {
        if (attack)
        {
            myAnimator.SetTrigger("quickAttack");
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            attack = true;
        }

    }
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {

            facingRight = !facingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;

            transform.localScale = scale;
            if (isGrounded)
                myRigidbody.velocity = Vector2.zero;

        }
    }

    private void ResetValues()
    {
        attack = false;
        isJumping = false;
    }
    
}
