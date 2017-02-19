using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "player")
        {
            GameData.current.scene = SceneManager.GetActiveScene().buildIndex;
            GameData.current.playerPosition = collider.transform.position;
            DataManager.SaveData();
        }
    }
}
