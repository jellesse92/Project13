using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelUpUIManager : MonoBehaviour {
    public Text level;
    public Text souls;
    public Text requiredSouls;
    public Text strength;
    public Text defense;
    public Text speed;

    PlayerData newPlayerStats;
    // Use this for initialization
    void Start() {
        UpdateUIText();
    }

    void UpdateUIText()
    {
        level.text = GameData.current.player1.level.ToString();
        souls.text = GameData.current.player1.souls.ToString();
        strength.text = GameData.current.player1.strength.ToString();
        defense.text = GameData.current.player1.defense.ToString();
        speed.text = GameData.current.player1.speed.ToString();
    }

    void UpdateUITextUsingNewStats()
    {
        level.text = GameData.current.player1.level.ToString();
        souls.text = GameData.current.player1.souls.ToString();
        strength.text = GameData.current.player1.strength.ToString();
        defense.text = GameData.current.player1.defense.ToString();
        speed.text = GameData.current.player1.speed.ToString();
    }

    int CalculateCost(int currentLevel)
    {
        if (currentLevel <= 1)
            return 100;
        else {
            int cost = CalculateCost(currentLevel - 1);
            return (int)(cost + cost * 0.1f);
        }
    }

    void UpdateNewPlayerStats()
    {
        newPlayerStats.level = Int32.Parse(level.text);
        newPlayerStats.souls = Int32.Parse(souls.text);
        newPlayerStats.strength = Int32.Parse(strength.text);
        newPlayerStats.defense = Int32.Parse(defense.text);
        newPlayerStats.speed = Int32.Parse(speed.text);
    }

    void IncreamentLevel()
    {
        level.text = (Int32.Parse(level.text) + 1).ToString();
    }


    void IncreamentStrength()

    {
        UpdateNewPlayerStats();
        int cost = CalculateCost(newPlayerStats.level);

        //if (cost <)
        strength.text = (Int32.Parse(strength.text) + 1).ToString();
    }
    
      
}
