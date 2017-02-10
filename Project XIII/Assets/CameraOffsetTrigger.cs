using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraOffsetTrigger : MonoBehaviour {
    public enum OffSetChoice
    {
        negativeYOffset, positiveYOffset, customYOffset
    }

    public OffSetChoice offsetChoice;
    public float numberCustomYOffset;
    public bool returnPreviousOffsetOnExit = true;

    float defaultYOffset;

    MultiplayerCamFollowScript cameraFollowScript;
    void Start()
    {
        cameraFollowScript = Camera.main.transform.parent.GetComponent<MultiplayerCamFollowScript>();
        defaultYOffset = cameraFollowScript.yOffset;
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if(player.tag == "Player")
        {
            UseOffSetChoice();
        }
    }

    void OnTriggerExit2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            if(returnPreviousOffsetOnExit)
                returnDefaultOffset();
        }
    }
    void UseOffSetChoice()
    {
        if (offsetChoice == OffSetChoice.negativeYOffset)
            NegativeYOffset();
        else if (offsetChoice == OffSetChoice.positiveYOffset)
            PositiveYOffset();
        else if (offsetChoice == OffSetChoice.customYOffset)
            CustomYOffset();
    }
    void NegativeYOffset()
    {
        if(cameraFollowScript.yOffset > 0)
            cameraFollowScript.yOffset *= -1;
    }

    void PositiveYOffset()
    {
        if (cameraFollowScript.yOffset < 0)
            cameraFollowScript.yOffset *= -1;
    }

    void CustomYOffset()
    {
        cameraFollowScript.yOffset = numberCustomYOffset;
    }

    void returnDefaultOffset()
    {
        cameraFollowScript.yOffset = defaultYOffset;
    }
}
