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

    //public string dashkey

}

public class PlayerInput : MonoBehaviour {

    private KeyConfig keyConfig = new KeyConfig();
    private KeyPress keyPress = new KeyPress();
    private int joystickNum = 0;

    void Start()
	{
        SetJoystickNum(0);  //Temporary
    }
    
    public void GetInput()
    {
        keyPress.horizontalAxisValue = Input.GetAxis(keyConfig.horizontalAxisName);
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.Space)) ||(joystickNum !=0 && Input.GetButtonDown(keyConfig.jumpButton)))
            keyPress.jumpPress = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.Z)) || (joystickNum != 0 && Input.GetButtonDown(keyConfig.quickAttackButton)))
            keyPress.quickAttackPress = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.X)) || (joystickNum != 0 && Input.GetButtonDown(keyConfig.heavyAttackButton)))
            keyPress.heavyAttackPress = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.C)) || (joystickNum != 0 && Input.GetButtonDown(keyConfig.blockButton)))
            keyPress.blockPress = true;
        /*
        if (Input.GetKeyDown(keyConfig.dashKey))
            keyPress.dashPress = true;
            */
    }

    public void ResetKeyPress()
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

    //Sets the joystick which will be controlling the attached player.
    //0 = keyboard + mouse input
    //1 - 11 = Joystick pad input
    public void SetJoystickNum(int num)
    {
        if (num == 0) return;
        
        if (num < 0 || num > 11)
        {
            joystickNum = 0;
            return;
        }

        joystickNum = num;

        keyConfig.jumpButton = num.ToString() + "_Circle";
        keyConfig.quickAttackButton = num.ToString() + "_X";
        keyConfig.heavyAttackButton = num.ToString() + "_Triangle";
        keyConfig.blockButton = num.ToString() + "_Square";
        //keyConfig.dashButton = ?


    }

}
