using UnityEngine;
using System.Collections;

public class AttackRangeDetect : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
            transform.parent.GetComponent<Enemy>().inAttackRange = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
            transform.parent.GetComponent<Enemy>().inAttackRange = false;
    }
}
