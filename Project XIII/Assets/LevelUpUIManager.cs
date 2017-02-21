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
    int cost;

    // Use this for initialization
    void Start() {
        UpdateUITextUsingGameData();
        newPlayerStats = GameData.current.player1;
        UpdateCostUI();
    }

    void UpdateUITextUsingGameData()
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
            return (int)(cost + cost * 0.4f);
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

    bool LevelUp() //return true if level up, return false if not enough souls to level
    {
        UpdateNewPlayerStats();
        cost = CalculateCost(newPlayerStats.level);
        if (newPlayerStats.souls >= cost)
        {
            newPlayerStats.level += 1;
            newPlayerStats.souls -= cost;
            UpdateCostUI();
            return true;
        }
        return false;
    }
   
    public void IncreamentStrength()
    {
        if (LevelUp())
        {
            newPlayerStats.strength += 1;
            UpdateUITextUsingNewStats();
        }
    }

    public void IncreamentDefense()
    {
        if (LevelUp())
        {
            newPlayerStats.defense += 1;
            UpdateUITextUsingNewStats();
        }
    }

    public void IncreamentSpeed()
    {
        if (LevelUp())
        {
            newPlayerStats.speed += 1;
            UpdateUITextUsingNewStats();
        }
    }

    public void UpdateCostUI()
    {
        cost = CalculateCost(newPlayerStats.level);
        requiredSouls.text = cost.ToString();
    }

    public void Accept()
    {
        GameData.current.player1 = newPlayerStats;
    }

    public void Cancel()
    {
        UpdateUITextUsingGameData();
    }
}
