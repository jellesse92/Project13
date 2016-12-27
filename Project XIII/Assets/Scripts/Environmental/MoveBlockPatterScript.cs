using UnityEngine;
using System.Collections;

public class MoveBlockPatterScript : MonoBehaviour
{

    [System.Serializable]
    public class MoveBlock                              //Contains info on which block ton control and when to trigger movement
    {
        public GameObject block;                        //Block to move
        public float delayStartMove;                    //Time to start after end of previous block sequence
    }

    [System.Serializable]
    public class BlockPattern                           //Pattern for current sequence of block movement
    {
        public MoveBlock[] moveBlock;
        public float delayNextPattern;                  //Amount of time to delay starting next sequence
    }

    public BlockPattern[] blockPattern;                 //Pattern of block movement

    //Private variables controlling flow of block patterns
    private int currentSequence = 0;                    //Current sequence being examined

    // Use this for initialization
    void Start()
    {
        StartPattern();
    }

    void StartPattern()
    {
        if (currentSequence >= blockPattern.Length)
            currentSequence = 0;
        foreach (MoveBlock movBlock in blockPattern[currentSequence].moveBlock)
        {
            StartCoroutine(waitToTrigger(movBlock.block, movBlock.delayStartMove));
        }
        currentSequence++;
        Invoke("StartPattern", blockPattern[currentSequence - 1].delayNextPattern);
    }

    IEnumerator waitToTrigger(GameObject block, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        block.GetComponent<SquishBlockScript>().TriggerMove();
    }

}


