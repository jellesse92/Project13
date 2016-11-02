﻿using UnityEngine;
using System.Collections;

public class KeyPress {
    public bool jumpPress;
    public bool quickAttackPress;
    public bool heavyAttackPress;
    public bool blockPress;
    public bool dashPress;
    public float horizontalAxisValue;
    public float verticalAxisValue;
}

[System.Serializable]
public class KeyConfig
{
    public string horizontalAxisName = "Horizontal_Key"; //name of the horizontal axis in the input manager
    public string verticalAxisName = "Vertical_Key";
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
        //SetJoystickNum(0);  //Temporary
    }
    
    public void GetInput()
    {

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

        keyConfig.horizontalAxisName = num.ToString() + "_LeftJoyStickX";
        keyConfig.verticalAxisName = num.ToString() + "_LeftJoyStickY";
        keyConfig.jumpButton = num.ToString() + "_Circle";
        keyConfig.quickAttackButton = num.ToString() + "_X";
        keyConfig.heavyAttackButton = num.ToString() + "_Triangle";
        keyConfig.blockButton = num.ToString() + "_Square";
        //keyConfig.dashButton = ?


    }

}