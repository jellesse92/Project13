using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public static GameData current;
    public bool isLoaded = false;
    public int scene = 1;

    public PlayerData player1 = new PlayerData();
    public PlayerData player2 = new PlayerData();
    public PlayerData player3 = new PlayerData();
    public PlayerData player4 = new PlayerData();
}

[System.Serializable]
public class PlayerData
{
    public float positionX = 0;
    public float positionY = 0;
    public int level = 1;
    public int souls = 0;
    public int strength = 1;
    public int defense = 1;
    public int speed = 1;
}