﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class to be inherited by all enemy scripts
public class Enemy : MonoBehaviour {

    //In-Game information                 
    public int health;                                  //Enemy health

    //Buffable elements
    public float speed;                                 //Speed of enemy
    public int attackPower;                             //Base attack power of enemy

    //Detection and Pursuit Variables
    bool isVisible;                                     //Determine if enemy is visible on screen
    bool inPursuit;                                     //Determine if enemy is in pursuit of player character
    GameObject target;                                  //Target to chase

    //Attack Variables
    bool inAttackRange;                                 //Detects if in range to begin attacking

    // Use this for initialization
    void Awake()
    {
        Reset();
        isVisible = true;
    }

    // Call when enemy becomes visible on screen
    void OnBecameVisible()
    {
        isVisible = true;
    }

    //Call when enemy becomes no longer visible on screen
    void OnBecameInvisible()
    {
        Reset();
    }

    //Call when enter a trigger field. If entering player trigger field and visible, activate pursuit status
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Detection Field")
        {
            inPursuit = true;
            target = col.transform.parent.gameObject;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //col.gameObject.GetComponent<PlayerCharacter>().TakeDamage(attackPower);
        }
    }

    //NOTE: Might change for efficiency issue
    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            //col.gameObject.GetComponent<PlayerCharacter>().TakeDamage(attackPower);
        }
    }

    //Resets position and alert status
    public void Reset()
    {
        isVisible = false;
        inPursuit = false;
        target = null;
        inAttackRange = false;
    }

    //Damage script to be applied when enemy takes damage
    public void Damage(int damage)
    {
        health -= damage;
    }

    //Gets if enemy is visible or not
    public bool GetVisibleState()
    {
        return isVisible;
    }

    //Get the info whether enemy is in pursuit or not
    public bool GetPursuitState()
    {
        return inPursuit;
    }

    //Sets whether or not enemy is in pursuit
    public void SetPursuitState(bool state)
    {
        inPursuit = state;
    }

    //Gets the character currently targeted
    public GameObject GetTarget()
    {
        return target;
    }

    //Sets the target player
    public void SetTarget(GameObject tar)
    {
        target = tar;
    }

    public void SetAttackInRange(bool b)
    {
        inAttackRange = b;
    }

    public bool GetAttackInRange()
    {
        return inAttackRange;
    }
}
