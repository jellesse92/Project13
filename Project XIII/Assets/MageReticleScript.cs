using UnityEngine;
using System.Collections;

public class MageReticleScript : MonoBehaviour {

    const float SPEED = .5f;

    PlayerInput input;
    Vector3 localPosition;
    bool checkedLastPos;

    void Start()
    {
        input = transform.parent.GetComponent<PlayerInput>();
        checkedLastPos = false;
    }

    void FixedUpdate()
    {

        float x = (Mathf.Abs(input.getKeyPress().horizontalAxisValue) > 0.05) ? input.getKeyPress().horizontalAxisValue : 0f;
        float y = (Mathf.Abs(input.getKeyPress().verticalAxisValue) > 0.05) ? input.getKeyPress().verticalAxisValue : 0f;

        Vector3 moveDir = new Vector3(x * SPEED, y * SPEED, 0f);

        transform.position = transform.position + moveDir;

        if (GetComponent<SpriteRenderer>().enabled)
        {
            checkedLastPos = false;
            localPosition = transform.position;
        }

        else
            if (!checkedLastPos)
            {
                checkedLastPos = true;
            }



    }

    void LateUpdate()
    {
        if(!GetComponent<SpriteRenderer>().enabled)
            transform.position = localPosition;
    }

}
