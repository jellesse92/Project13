using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public int PlayerNumber = -1;
	public bool sidePerspective;
	public KeyCode Up;
	public KeyCode Down;
	public KeyCode Left;
	public KeyCode Right;
	public KeyCode Jump;
    public KeyCode Attack1;
    public KeyCode Attack2;
    public KeyCode Block;
    public PlayerCharacter character;
    Animator anim;

	void Start()
	{
        sidePerspective = true;
        SetPlayerNumber(-1);
	}

    public void SetPlayerNumber(int num)
    {
        PlayerNumber = num;
        SetInput();
        //anim.SetInteger("x", 1)
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
				character = new CowBoyClass();
				break;
			default:
				character = new CowBoyClass();
				break;
		}
	}
	void swapPerspective()
	{
		sidePerspective = false;
	}
	void Update()
	{
        if (PlayerNumber != -1 || true) //or true for testing purposes.
        {
            SelectClass(0);
            if (sidePerspective == false && (Input.GetKeyDown(Up) || Input.GetKeyDown(Down)))
            {
                var move = new Vector3(0, Input.GetKeyDown(Up)? 1 : -1, 0);
                transform.position += move * 2 * Time.deltaTime;
            }
            else if (Input.GetKeyDown(Left) || Input.GetKeyDown(Right))
            {
                var move = new Vector3(Input.GetKeyDown(Left) ? -1 : 1, 0, 0);
                transform.position += move * 4 * Time.deltaTime;
            }
            if (Input.GetKeyDown(Jump))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                print("Jumping");
            }
        }

    }
}
