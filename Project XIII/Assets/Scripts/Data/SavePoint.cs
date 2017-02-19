using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Save");
            if (GameData.current == null)
                GameData.current = new GameData();
            GameData.current.scene = SceneManager.GetActiveScene().buildIndex;
            GameData.current.playerPositionX = collider.transform.position.x;
            GameData.current.playerPositionY = collider.transform.position.y;
            DataManager.SaveData();
        }
    }
}
