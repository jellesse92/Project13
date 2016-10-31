using UnityEngine;
using System.Collections;

public class KeyPress {
    public bool jumpPress;
    public bool quickAttackPress;
    public bool heavyAttackPress;
    public bool blockPress;
    public bool dashPress;
    public float horizontalAxisValue;
}

[System.Serializable]
public class KeyConfig
{
    public string horizontalAxisName = "Horizontal"; //name of the horizontal axis in the input manager

    public string jumpButton = "1_Circle";
    public string quickAttackButton = "1_X";
    public string heavyAttackButton = "1_Triangle";
    public string blockButton = "1_Square";
    public string dashButton;

}

public class PlayerInput : MonoBehaviour {

    public int joystickNumber = 0;          //0 refers to keyboard + mouse input. 1-11 refers to joystick number
    KeyConfig playerKeyConfig = new KeyConfig();
    private KeyConfig keyConfig = new KeyConfig();
    private int playerNumber;

    private KeyPress keyPress = new KeyPress();
    
    void Start()
	{
        playerNumber = GetComponent<PlayerProperties>().GetPlayerNumber();
        SetController(joystickNumber);
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        ResetKeyPress();
    }

    void HandleInput()
    {
        keyPress.horizontalAxisValue = Input.GetAxis(keyConfig.horizontalAxisName);
        Debug.Log(joystickNumber);
        Debug.Log(keyConfig.jumpButton);

        if((joystickNumber == 0 && Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown(keyConfig.jumpButton)))
        {
            keyPress.jumpPress = true;
        }

        if ((joystickNumber == 0 && Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown(keyConfig.quickAttackButton)))
            keyPress.quickAttackPress = true;
        if ((joystickNumber == 0 && Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown(keyConfig.heavyAttackButton)))
            keyPress.heavyAttackPress = true;
        if ((joystickNumber == 0 && Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(keyConfig.blockButton)))
            keyPress.blockPress = true;
        /*
        if (Input.GetKeyDown(keyConfig.dashKey))
            keyPress.dashPress = true;
            */
    }

    void ResetKeyPress()
    {
        keyPress.jumpPress = false;
        keyPress.quickAttackPress = false;
        keyPress.heavyAttackPress = false;
        keyPress.blockPress = false;
        keyPress.dashPress = false;
    }

    public KeyPress getKeyPress()
    {
        return keyPress;
    }

    //Sets controller number. 0 refers to keyboard+mouse input, 1-11 refers to joystick number
    public void SetController(int num)
    {
        if (num == 0)       //Player is using keyboard
            return;

        joystickNumber = num;
        playerKeyConfig.horizontalAxisName = num.ToString() + "LeftJoystickX";
        playerKeyConfig.jumpButton = num.ToString() + "_Circle";
        playerKeyConfig.quickAttackButton = num.ToString() + "_X";
        playerKeyConfig.heavyAttackButton = num.ToString() + "_Triangle";
        playerKeyConfig.blockButton = num.ToString() + "_Square";

    }

}
