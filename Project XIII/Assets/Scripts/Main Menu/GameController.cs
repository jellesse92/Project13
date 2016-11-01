using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Any changes to this script have to be given a pass-through function in FindDontDestroy
    /// </summary>
    
    public struct PlayerInfo
    {
        bool Alive;
        int PlayerNumber;
        int LivesLeft;
        int CurrentHealth;
        int PrimaryAP; //Attack Power
        int SecondaryAP;
        int Cash;
        int MaxHealth;
        float PlayerSpeed;
        float PrimaryAS; //Attack Speed
        float SecondaryAS;
    }

    public PlayerInfo[] playersInfos;
    public int[] PlayerCharacters;
    public int PlayerCount = 0;
    public bool IsMusicOn = true;
    public bool IsSfxOn = true;
    public int Volume = 100;

    public static GameController Instance
    {
        get;
        set;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;

        //Destroys copy of this on scene
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);
    }
    
    public void SetPlayerCount(int count)
    {
        PlayerCount = count;
        PlayerCharacters = new int[count];
    } 


    //1 = Swordsman; 2 = Gunner; 3 = Mage; 4 = Mech
    public void SetChar(int player, int CharType)
    {
        PlayerCharacters[player] = CharType;
    }

    public void SetInput(int player, int joystickNum)
    {
        PlayerCharacters[player] = joystickNum;
    }
}