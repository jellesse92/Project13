using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public static GameData current;
    public bool isLoaded = false;
    public int scene;
    public float playerPositionX;
    public float playerPositionY;
}
