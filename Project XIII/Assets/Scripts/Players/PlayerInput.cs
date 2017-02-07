﻿using UnityEngine;
using System.Collections;

public class KeyPress {
    public bool jumpPress;
    public bool quickAttackPress;
    public bool heavyAttackPress;
    public bool blockPress;
    public bool dashPress;
    public bool rightTriggerPress;
    public float horizontalAxisValue;
    public float verticalAxisValue;

    //Held buttons
    public bool quickAttackReleased;
    public bool heavyAttackReleased;
    public bool jumpReleased;

}

[System.Serializable]
public class KeyConfig
{
    public string horizontalAxisName = "Horizontal_Key"; //name of the horizontal axis in the input manager
    public string verticalAxisName = "Vertical_Key";
    public string jumpButton = "1_X";
    public string quickAttackButton = "1_Square";
    public string heavyAttackButton = "1_Triangle";
    public string specialButton = "1_Circle";
    public string dashAxis = "1_RightTrigger";

    //public string dashkey

}

public class PlayerInput : MonoBehaviour {

    private KeyConfig keyConfig = new KeyConfig();
    private KeyPress keyPress = new KeyPress();
    private int joystickNum = 0;
    private bool inputEnabled = true;
    private bool rightTriggerReleased = true;

    void Start()
	{

    }
    
    public void GetInput()
    {
        if (!inputEnabled || joystickNum == -1)
            return;

        if(joystickNum == 0)
        {
            keyPress.horizontalAxisValue = Input.GetAxis(keyConfig.horizontalAxisName);
            keyPress.verticalAxisValue = Input.GetAxis(keyConfig.verticalAxisName);
        }
        else
        {
            //For bad calibration
            float x = (Mathf.Abs(Input.GetAxis(keyConfig.horizontalAxisName)) > 0.06) ? Input.GetAxis(keyConfig.horizontalAxisName) : 0f;
            float y = (Mathf.Abs(Input.GetAxis(keyConfig.verticalAxisName)) > 0.06) ? Input.GetAxis(keyConfig.verticalAxisName) : 0f;

            keyPress.horizontalAxisValue = x;
            keyPress.verticalAxisValue = y;
        }

        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.Space)) ||(joystickNum !=0 && Input.GetButtonDown(keyConfig.jumpButton)))
            keyPress.jumpPress = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.Z)) || (joystickNum != 0 && Input.GetButtonDown(keyConfig.quickAttackButton)))
            keyPress.quickAttackPress = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.X)) || (joystickNum != 0 && Input.GetButtonDown(keyConfig.heavyAttackButton)))
            keyPress.heavyAttackPress = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.C)) || (joystickNum != 0 && rightTriggerReleased &&(Input.GetAxis(keyConfig.dashAxis) > .5f)))
            keyPress.dashPress = true;
        if (joystickNum != 0 && Input.GetAxis(keyConfig.dashAxis) < .1f)
            rightTriggerReleased = true;
        if ((joystickNum == 0 && Input.GetKeyDown(KeyCode.V)))//|| (joystickNum != 0 && Input.GetButtonDown(keyConfig.blockButton)
            keyPress.blockPress = true;
        if ((joystickNum == 0 && Input.GetKeyUp(KeyCode.Z)) || (joystickNum != 0 && Input.GetButtonUp(keyConfig.quickAttackButton)))
            keyPress.quickAttackReleased = true;
        if ((joystickNum == 0 && Input.GetKeyUp(KeyCode.X)) || (joystickNum != 0 && Input.GetButtonUp(keyConfig.heavyAttackButton)))
            keyPress.heavyAttackReleased = true;
        if ((joystickNum == 0 && Input.GetKeyUp(KeyCode.Space)) || (joystickNum != 0 && Input.GetButtonUp(keyConfig.jumpButton)))
            keyPress.jumpReleased = true;
    }

    public void ResetKeyPress()
    {
        keyPress.jumpPress = false;
        keyPress.quickAttackPress = false;
        keyPress.heavyAttackPress = false;
        keyPress.blockPress = false;
        keyPress.dashPress = false;
        keyPress.quickAttackReleased = false;
        keyPress.heavyAttackReleased = false;
        keyPress.jumpReleased = false;
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
        if(num == -1)
        {
            joystickNum = num;
            return;
        }
        if (num == 0) return;
        
        if (num < 0 || num > 11)
        {
            joystickNum = 0;
            return;
        }

        joystickNum = num;

        keyConfig.horizontalAxisName = num.ToString() + "_LeftJoyStickX";
        keyConfig.verticalAxisName = num.ToString() + "_LeftJoyStickY";
        keyConfig.jumpButton = num.ToString() + "_X";
        keyConfig.quickAttackButton = num.ToString() + "_Square";
        keyConfig.heavyAttackButton = num.ToString() + "_Triangle";
        keyConfig.specialButton = num.ToString() + "_Circle";
        keyConfig.dashAxis = num.ToString() + "_RightTrigger";


    }

    public void SetInputActive(bool b)
    {
        inputEnabled = b;
    }

    public bool InputActiveState()
    {
        return inputEnabled;
    }

    public int GetJoystick()
    {  
        return joystickNum;
    }
}
