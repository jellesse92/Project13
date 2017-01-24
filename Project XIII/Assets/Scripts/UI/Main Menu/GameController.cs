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

    public struct CharacterSetting
    {
        public int player;                          //Player controlling the character
        public int joystickNum;                     //Inputs the character should be following
    }

    public PlayerInfo[] playersInfos;
    public CharacterSetting[] PlayerCharacters = new CharacterSetting[4];   //Refers to character information instead of player information
    public int PlayerCount = 0;
    public bool IsMusicOn = true;
    public bool IsSfxOn = true;
    public int musicVolume = 100;
    public int sfxVolume = 100;
    private AudioSource[] sfxObjects;
    private MusicManager music;

    public static GameController Instance
    {
        get;
        set;
    }

    void Awake()
    {
        music = FindObjectOfType<MusicManager>();
        sfxObjects = FindObjectsOfType<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;

        //Destroys copy of this on scene
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);

        for (int i = 0; i < 4; i++)
            PlayerCharacters[i].player = -1;

        //Default settings. 1 Player set to gunner with keyboard + mouse input
        //SetPlayer(0, 2, 0);

    }


    //1 = Swordsman; 2 = Gunner; 3 = Mage; 4 = Mech
    public void SetPlayer(int player, int CharType, int controlNum)
    {
        if (CharType == -1)
            return;
        PlayerCharacters[CharType - 1].player = player + 1;
        PlayerCharacters[CharType - 1].joystickNum = controlNum;
    }

    public void AssignInputs(Transform playerList)
    {

        int numberOfAvailablePlayers = playerList.transform.childCount;
        int numberOfSelectedPlayers = numberOfAvailablePlayers;

        for (int i = 0; i < numberOfAvailablePlayers; i++)
        {
            if (PlayerCharacters[i].player == -1)
                numberOfSelectedPlayers -= 1;
        }

        if (numberOfSelectedPlayers == 0)
            return;

        for (int i = 0; i < numberOfAvailablePlayers; i++)
        {
            Transform character = playerList.GetChild(i);
            if (PlayerCharacters[i].player == -1)
                character.gameObject.SetActive(false);
            else
            {
                character.GetComponent<PlayerProperties>().playerNumber = PlayerCharacters[i].player;
                character.GetComponent<PlayerInput>().SetJoystickNum(PlayerCharacters[i].joystickNum);
            }

        }
    }

    public void ChangeeMusicVol(int amnt)
    {
        musicVolume += (musicVolume + amnt > 100 ? 0 : amnt);
        IsMusicOn = musicVolume > 0;
        if (!IsMusicOn)
            musicVolume = 0;
        music.SetMusicVolume(musicVolume / 10);
    }
    

    public void ChangeSFXVol(int amnt)
    {
        foreach (AudioSource sfxObject in sfxObjects)
        {
            sfxVolume += (sfxVolume + amnt > 100 ? 0 : amnt);
            IsSfxOn = sfxVolume > 0;
            if (!IsSfxOn)
                sfxVolume = 0;
            sfxObject.volume = sfxVolume;
        }
    }
    
}