using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    //In-Game information
    public int fullHealth;                              //Full health to reset to                      
    public int health;                                  //Enemy health

    //Detection Variables
    public bool isVisible;                              //Determine if enemy is visible on screen
    public bool inPursuit;                              //Determine if enemy is in pursuit of player character
    public GameObject target;                           //Target to chase

    // Use this for initialization
    void Awake()
    {
        Reset();
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

    //Resets position and alert status
    public void Reset()
    {
        isVisible = false;
        inPursuit = false;
    }
}
