using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecificDetection : MonoBehaviour {

    Enemy enemyScript;

    private void Awake()
    {
        enemyScript = transform.parent.GetComponent<Enemy>();
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log(collision.name);
            enemyScript.SetTarget(collision.gameObject);
            enemyScript.AddTargetList(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enemyScript.RemoveTargetList(collision.gameObject);
        }
    }
}
