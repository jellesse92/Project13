using UnityEngine;
using System.Collections;

public class FreezeEnemyScript : MonoBehaviour {

    public bool startFrozen = false;

    void Start()
    {
        if(startFrozen)
            FreezeEnemies();
    }

	public void FreezeEnemies()
    {
        foreach(Transform child in transform)
        {
            if (child.gameObject.tag == "Enemy")
                child.gameObject.GetComponent<Enemy>().SetFrozenState(true);
        }
    }

    public void UnfreezeEnemies()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Enemy")
                child.gameObject.GetComponent<Enemy>().SetFrozenState(false);
        }
    }
}
