using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnrageZoneScript : MonoBehaviour {

    HashSet<GameObject> playersinRange = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("test ");
        if(col.tag == "Player")
        {
            playersinRange.Add(col.gameObject);

        }
    }

    public void EnrageEnemies()
    {
        Random random = new Random();
        foreach (Transform child in transform)
        {

            


            //child.gameObject.GetComponent<Enemy>().SetTarget(col.gameObject);
            child.gameObject.GetComponent<Enemy>().SetPursuitState(true);
        }
    }
}
