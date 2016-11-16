using UnityEngine;
using System.Collections;

public class SwordsmanMelee : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<Enemy>().Damage(20, 1.2f);
            Debug.Log("hit");
        }

    }
}
