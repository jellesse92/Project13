using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
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
    }
    
    public void SetPlayerCount(int count)
    {
        PlayerCount = count;
        PlayerCharacters = new int[count];
    } 
    public void SetChar(int CharType)
    {
        for(int i = 0; i < PlayerCount; i++)
        {
            if (PlayerCharacters[i] == 0)
            {
                PlayerCharacters[i] = CharType;
            }
        }
    }
}