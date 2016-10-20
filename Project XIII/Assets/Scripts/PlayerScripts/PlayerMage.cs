﻿using UnityEngine;
using System.Collections;

public class PlayerMage : MonoBehaviour {

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    private bool facingRight;
    private bool quickAttack;
    private bool heavyAttack;
    private bool isGrounded;
    private bool isJumping = false;
    private bool jumpPressed = false;
   
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
            if(!isJumping)
                Flip(horizontal);
            HandleAttacks();
        }
        else
            myRigidbody.velocity = Vector2.zero;

        ResetValues();

    }

    private void HandleMovement(float horizontal)
    {
        if (jumpPressed && !isJumping)
        {
            isJumping = true;
            if (Mathf.Abs(horizontal) > 0.1)
                myAnimator.SetTrigger("jumpForward");
            else
                myAnimator.SetTrigger("jumpIdle");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        myRigidbody.velocity = new Vector2(horizontal * movementSpeed ,myRigidbody.velocity.y);
        myAnimator.SetFloat("speed",Mathf.Abs(horizontal));
        
    }
    
    private void HandleAttacks()
    {
        if (quickAttack || heavyAttack)
        {
            if (quickAttack)
            {
                if (isJumping)
                    myAnimator.SetTrigger("airQuickAttack");
                else
                    myAnimator.SetTrigger("quickAttack");
            }
            if (heavyAttack)
            {
                if (isJumping)
                    myAnimator.SetTrigger("airHeavyAttack");
                else
                    myAnimator.SetTrigger("heavyAttack");
            }
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            quickAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            heavyAttack = true;
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
        quickAttack = false;
        heavyAttack = false;
        jumpPressed = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            Debug.Log("collison enter");
            myAnimator.SetTrigger("landing");
            isJumping = false;
        }
    }
}
