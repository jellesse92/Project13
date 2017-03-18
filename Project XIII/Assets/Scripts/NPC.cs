using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public GameObject interactionPrompt;
    public int[] textAssetIndex;                                //Text assets to play
    CutsceneManager cutsceneScript;

    int count = 0;

    HashSet<GameObject> playerHash = new HashSet<GameObject>();

    private void Start()
    {
        cutsceneScript = GameObject.FindGameObjectWithTag("In Game UI").GetComponentInChildren<CutsceneManager>();
        interactionPrompt.SetActive(false);
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !playerHash.Contains(collision.gameObject))
        {
            playerHash.Add(collision.gameObject);
            interactionPrompt.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && playerHash.Contains(collision.gameObject))
        {
            playerHash.Remove(collision.gameObject);
            if (playerHash.Count <= 0)
                interactionPrompt.SetActive(false);
        }
    }

    public void ActivateInteraction()
    {
        int length = textAssetIndex.Length;
        if(length > 0)
        {
            cutsceneScript.ActivateCutscene(textAssetIndex[count]);
            if (count < length - 1)
                count++;
        }
    }
}
