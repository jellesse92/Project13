using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyInfiniteSpawnerScript : MonoBehaviour {

    public GameObject enemy;
    public int spawnAmount;
    public int initialSpawn = 0;
    public float spawnRate;
    public int spawnPerSpawning;                //Amount to spawn when spawning enemies
    

    HashSet<GameObject> enemyHash = new HashSet<GameObject>();

	// Use this for initialization
	void Awake () {

	    for(int i = 0; i < spawnAmount; i++)
        {
            GameObject newSpawn = (GameObject)Instantiate(enemy);
            newSpawn.transform.parent = this.transform;
            newSpawn.SetActive(false);
            enemyHash.Add(newSpawn);
        }

        for(int i = 0; i < initialSpawn; i++)
        {
            GameObject newSpawn = getSpawn();
            if (newSpawn == null)
                break;

            newSpawn.GetComponent<Enemy>().Reset();
            newSpawn.transform.position = transform.position;
            newSpawn.SetActive(true);
        }

        InvokeRepeating("Spawn", spawnRate, spawnRate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Spawn()
    {
        for(int i =0; i < spawnPerSpawning; i++)
        {
            GameObject newSpawn = getSpawn();
            if (newSpawn == null)
                break;

            newSpawn.GetComponent<Enemy>().Reset();
            newSpawn.transform.position = transform.position;
            newSpawn.SetActive(true);
        }
    }


    GameObject getSpawn()
    {
        GameObject spawn = null;

        foreach(GameObject enemy in enemyHash)
        {
            if (!enemy.activeSelf || enemy.gameObject.layer == 14)
            {
                spawn = enemy;
                spawn.GetComponent<Enemy>().Reset();
                return spawn;
            }
            else if(enemy.GetComponent<Enemy>().health <= 0)
            {
                enemy.GetComponent<Enemy>().Reset();
                spawn = enemy;
            }
        }

        return spawn;
    }
}
