using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public static GameData current;
    public bool loaded;
    public int scene;
    public float playerPositionX;
    public float playerPositionY;
}
