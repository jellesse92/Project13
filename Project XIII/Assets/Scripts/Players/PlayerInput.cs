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
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode quickAttackKey = KeyCode.Z;
    public KeyCode heavyAttackKey = KeyCode.X;
    public KeyCode blockKey;
    public KeyCode dashKey;
}

public class PlayerInput : MonoBehaviour {

    public KeyConfig player1;
    public KeyConfig player2;

    public KeyConfig player3;
    public KeyConfig player4;

    private KeyConfig keyConfig = new KeyConfig();
    private int playerNumber;

    private KeyPress keyPress = new KeyPress();
    
    void Start()
	{
        keyConfig = player1;
        playerNumber = GetComponent<PlayerProperties>().GetPlayerNumber();
    }
    
    public void GetInput()
    {
        keyPress.horizontalAxisValue = Input.GetAxis(keyConfig.horizontalAxisName);
        if (Input.GetKeyDown(keyConfig.jumpKey))
            keyPress.jumpPress = true;
        if (Input.GetKeyDown(keyConfig.quickAttackKey))
            keyPress.quickAttackPress = true;
        if (Input.GetKeyDown(keyConfig.heavyAttackKey))
            keyPress.heavyAttackPress = true;
        if (Input.GetKeyDown(keyConfig.blockKey))
            keyPress.blockPress = true;
        if (Input.GetKeyDown(keyConfig.dashKey))
            keyPress.dashPress = true;
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

}
