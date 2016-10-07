using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlertFieldScript : MonoBehaviour {

    List<GameObject> allies;                            //List of nearby allies

    void Awake()
    {
        allies = new List<GameObject>();
    }

	void OnTriggerEnter2D(Collider2D col)
    {
        if(transform.parent.GetComponent<Enemy>().isVisible)
            if (col.tag == "Enemy")
            {
                //Adds list of nearby enemies to alert upon spottings player
                allies.Add(col.gameObject);

                //Alerts fellow enemies as it passes by while in pursuit
                if (transform.parent.GetComponent<Enemy>().inPursuit)
                    if (col.tag == "Enemy" && col.gameObject.GetComponent<Enemy>().target == null)
                    {
                        col.gameObject.GetComponent<Enemy>().target = col.gameObject;
                        col.gameObject.GetComponent<Enemy>().inPursuit = true;
                    }
            }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (transform.parent.GetComponent<Enemy>().isVisible)
            if (allies.Contains(col.gameObject))
                allies.Remove(col.gameObject);
    }


    public void AlertAllies()
    {
        foreach (GameObject ally in allies)
        {
            ally.GetComponent<Enemy>().target = transform.parent.GetComponent<Enemy>().target;
            ally.GetComponent<Enemy>().inPursuit = true;
        }
    }
}
