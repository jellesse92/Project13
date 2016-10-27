using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    const int BASE_TOP_SPEED = 2;                               //Base speed for top down perspective
    const int BASE_SIDE_SPEED = 8;                              //Base speed for side-scroll perspective

    Animator anim;
    bool isFacingRight;


    public Transform projectiles;
    public int PlayerNumber = -1;
	public bool sidePerspective;
    private bool isJumping = false;
    public KeyCode Up;
	public KeyCode Down;
	public KeyCode Left;
	public KeyCode Right;
	public KeyCode Jump;
    public KeyCode Attack1;
    public KeyCode Attack2;
    public KeyCode Block;
    public PlayerCharacter character = null;

    private bool isAttacking;
    int AttackPower;
    int AttackSpeed;
    char direction = 'R'; //direction player is facing UDLR

    void Start()
	{
        anim = GetComponent<Animator>();
        isFacingRight = true;
        SetPlayerNumber(0);
	}

    public void SetPlayerNumber(int num)
    {
        PlayerNumber = num;
        SetInput();
    }
    private void SetInput()
    {
        switch (PlayerNumber)
        {
            case 0:
                Up = KeyCode.UpArrow;
                Down = KeyCode.DownArrow;
                Left = KeyCode.LeftArrow;
                Right = KeyCode.RightArrow;
                Jump = KeyCode.Space;
                Attack1 = KeyCode.A;
                Attack2 = KeyCode.S;
                Block = KeyCode.D;
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                Up = KeyCode.UpArrow;
                Down = KeyCode.DownArrow;
                Left = KeyCode.LeftArrow;
                Right = KeyCode.RightArrow;
                Jump = KeyCode.Space;
                Attack1 = KeyCode.A;
                Attack2 = KeyCode.S;
                Block = KeyCode.D;
                break;
        }
    }
	public void SelectClass(int choice  = 0)
	{
		switch (choice)
		{
			case 0:
				character = GetComponent<CowBoyClass>();
				break;
			default:
				character = GetComponent<CowBoyClass>();
				break;
		}
        anim = GetComponent<Animator>();
	}
	void swapPerspective()
	{
		sidePerspective = !sidePerspective;

        if (sidePerspective)
            isFacingRight = true;
        else
            isFacingRight = false;
	}
    void Update()
    {
        if (PlayerNumber != -1 || true) //or true for testing purposes.
        {


            int dir = 0;
            if (character == null)
            {
                SelectClass(0);
            }
            if (sidePerspective == false && (Input.GetKey(Up) || Input.GetKey(Down)))
            {
                dir = Input.GetKey(Up) ? 1 : -1;
                var move = new Vector3(0, dir, 0);
                transform.position += move * BASE_TOP_SPEED * Time.deltaTime;


                Debug.Log("Dir:" + dir);
                AnimateTopDown(0, dir);

            }
            else if (Input.GetKey(Left) || Input.GetKey(Right))
            {

                dir = Input.GetKey(Left) ? -1 : 1;

                if ((dir == 1 && !isFacingRight) || (dir == -1 && isFacingRight))
                    Flip();

                var move = new Vector3(dir, 0, 0);
                direction = dir == -1 ? 'L' : 'R';

                if (sidePerspective)
                {
                    transform.position += move * BASE_SIDE_SPEED * Time.deltaTime;
                    if(!isJumping)
                        AnimateSideScroll(1f);
                }

                else
                {
                    transform.position += move * BASE_TOP_SPEED * Time.deltaTime;
                    AnimateTopDown(1, 0);
                }

            }
            if (dir == 0)
            {
                if (!sidePerspective)
                    AnimateTopDown(0, 0);
                else
                    AnimateSideScroll(0f);
            }
            if (sidePerspective && Input.GetKeyDown(Jump) && !isJumping)
            {
                isJumping = true;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                if (dir != 0)
                    anim.SetTrigger("jumpForward");
                else
                    anim.SetTrigger("jumpIdle");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                //AnimateAttack
                AttackPower = character.Attack2D(0, direction);
                isAttacking = AttackPower < 0 ? false : true;
                anim.SetTrigger("quickAttack");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                //AnimateAttack
                AttackPower = character.Attack2D(1, direction);
                isAttacking = AttackPower < 0 ? false : true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                //AnimateBlock
                print("Blocking");
                character.Block(direction);
            }
        }
    }


    public void TakeDamage(int dmg, int knockBackForce = 0)
    {
        if (!Input.GetKeyDown(KeyCode.D))
        {
            character.TakeDamage(dmg);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Impulse);
            if (character.GetCurrentHealth() <= 0)
            {
                //Animate Death
                //Reset Location
                character.PlayerDeath();
            }
        } 

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Ground")
        {
            isJumping = false;
            anim.SetTrigger("landing");
        }
        if(isAttacking && col.collider.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().Damage(AttackPower);
            isAttacking = false;
        }
    }


    void AnimateTopDown(int x, int y)
    {
        anim.SetInteger("velocityX", x);
        anim.SetInteger("velocityY", y);
    }

    void AnimateSideScroll(float x)
    {
        anim.SetFloat("speed", x);
    }

    //Flips character to match direction of movement
    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
    }
}
