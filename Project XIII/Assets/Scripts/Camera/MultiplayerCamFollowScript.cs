using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerCamFollowScript : MonoBehaviour {

    const float DEFAULT_ORTHO_SIZE = 8f;
    const float DEFAULT_ORTHO_SIZE_3D = 20f;
    const float MAX_ORTHO_SIZE = 10f;
    const float MAX_ORTHO_SIZE_3D = 40f;

    const float ZOOM_OUT_DELTA = .01f;                         //Amount to zoom when zooming in or out per update
    const float ZOOM_IN_DELTA = .003f;

    const float CROUCHING_INCREAMENT = 4;

    public float xOffset = 6;
    public float yOffset = 3;

    float zoomMultiplier = 1f;
    float followDelay = .8f;

    HashSet<GameObject> players = new HashSet<GameObject>();

    public bool in2DMode = true;
    public float lowestPointY = 0f;                             //Lowest point of the map the camera should be able to show
    public float lowestPointX = 0f;                             //Leftmost point of the map the camera should be able to show

    float lastOrthographicSize = 0f;                            //Keeps track of the last orthographic size
    bool orthoTransitioning = false;
    bool orthoForced = false;                                   //If orthographic size has to be forced  
    bool crouching = false;
    //Cutscene variables
    bool forcingMovement = false;                               //For cutscene manager to force movement of camera
    bool targetSet = false;                                     //For if there is a target destination
    float forcedOrthoSize = DEFAULT_ORTHO_SIZE;                 //Orthographic size to be set to when forced
    Transform forcedDestination;                                //Destination of camera

    Camera cam;
    public GameObject cameraWall;

    Vector3 velocity = Vector3.zero;
    public float dampTime = 0.15f;

    // Use this for initialization
    void Start () {
        GameObject[] tempPlayerFind = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in tempPlayerFind)
        {
            if(p.GetComponent<PlayerInput>().GetJoystick() != -1)
                players.Add(p);
        }
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        int activePlayers = ActivePlayerCount();
        if (forcingMovement)
            CutsceneCamera();
        else if (activePlayers == 1)
            SinglePlayerCamera();
        else if (activePlayers > 1)
            MultiplayerCamera();
	}

    void CutsceneCamera()
    {
        if (orthoForced)
            SetOrthographicSize(forcedOrthoSize);
        if (targetSet)
        {
            if(forcedDestination.position.x == transform.position.x && forcedDestination.position.y == transform.position.y)
                EndForceDestination();
            DampMotion(forcedDestination);
        }
            
    }

    void SinglePlayerCamera()
    {
        Transform singlePlayerTrans = transform;

        SetCameraWallActive(false);

        if (orthoForced)
            SetOrthographicSize(forcedOrthoSize);
        else
            SetOrthographicSize(0);

        foreach (GameObject player in players)
        {
            if (player.activeSelf && player.GetComponent<PlayerInput>().GetJoystick()!=-1)
            {
                singlePlayerTrans = player.transform;
                break;
            }
        }

        DampMotion(singlePlayerTrans);
    }

    void DampMotion(Transform target)
    {        
        Vector3 destination = target.position;
        if (target.localScale.x < 0)
            destination.x -= xOffset;
        else
            destination.x += xOffset;

        if(crouching)
            destination.y += yOffset - CROUCHING_INCREAMENT;
        else
            destination.y += yOffset;     
        destination.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }

    public void SetCrouch(bool value)
    {
        crouching = value;
    }
    void MultiplayerCamera()
    {
        Vector3 midpoint = GetPlayersMidpoint();

        SetCameraWallActive(true);

        if (midpoint != new Vector3())
        {
            float distance = GetDistance();
            distance = Mathf.Max(1f, distance);

            Vector3 cameraDestination;

            cameraDestination = midpoint - transform.forward * distance * zoomMultiplier;

            if (orthoForced)
                SetOrthographicSize(forcedOrthoSize);
            else
                SetOrthographicSize(distance);

            if(in2DMode)
                cameraDestination.y = Mathf.Max(lowestPointY + cam.orthographicSize-5, cameraDestination.y);

            if (crouching)
                cameraDestination.y -= (5f + CROUCHING_INCREAMENT);
            else
                cameraDestination.y -= 5f;
            
            Vector3 newCameraDestination = Vector3.Slerp(transform.position, cameraDestination, followDelay);
            newCameraDestination.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, newCameraDestination, ref velocity, dampTime);

            if ((cameraDestination - transform.position).magnitude <= 0.05f)
                transform.position = cameraDestination;
        }

    }

    void SetCameraWallActive(bool b)
    {
        if (cameraWall != null)
            cameraWall.SetActive(b);
    }

    public void ForceOrthographicSize(float size)
    {
        forcedOrthoSize = size;
        orthoForced = true;
    }

    public void DeactivateForceOrthographicSize()
    {
        orthoForced = false;
        targetSet = false;
    }

    public void ForceDestination(Transform target)
    {
        forcingMovement = true;
        targetSet = true;
        forcedDestination = target;
    }

    public void EndForceDestination()
    {
        targetSet = false;
    }

    public void ForceInstantSetOrthographicSize(float f)
    {

        forcedOrthoSize = f;
        lastOrthographicSize = f;
        orthoForced = true;
        cam.orthographicSize = f;
    }

    //Sets orthographic size of camera based on given parameter f
    void SetOrthographicSize(float f)
    {
        float size;

        if (in2DMode)
        {
            if (orthoForced)
                size = f;
            else
            {
                size = Mathf.Max(f, DEFAULT_ORTHO_SIZE);
                size = Mathf.Min(size, MAX_ORTHO_SIZE);
            }

            if ((cam.orthographicSize > size))
                StartCoroutine(SmoothOrthograpicTransition(size));
            else if ((cam.orthographicSize < size))
                StartCoroutine(SmoothOrthograpicExpand(size));
            else if(!orthoForced)
                cam.orthographicSize = size;
            
        }
        else
        {
            size = Mathf.Max(f, DEFAULT_ORTHO_SIZE_3D);
            size = Mathf.Min(size, MAX_ORTHO_SIZE_3D);
            cam.orthographicSize = size;
        }

    }

    //Smooths out the orthographic size transition
    IEnumerator SmoothOrthograpicTransition(float size)
    {
        lastOrthographicSize = size;
        orthoTransitioning = true;

        while(cam.orthographicSize > size)
        {

            if (cam.orthographicSize - size < ZOOM_IN_DELTA)
            {
                cam.orthographicSize = size;
                orthoTransitioning = false;
                break;
            }

            cam.orthographicSize -= ZOOM_IN_DELTA;
        
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator SmoothOrthograpicExpand(float size)
    {
        lastOrthographicSize = size;
        orthoTransitioning = true;

        while (cam.orthographicSize < size)
        {

            if ( size - cam.orthographicSize < ZOOM_OUT_DELTA)
            {
                cam.orthographicSize = size;
                orthoTransitioning = false;
                break;
            }

            cam.orthographicSize += ZOOM_OUT_DELTA;

            yield return new WaitForSeconds(1.0f);
        }
    }

    //Get midpoint position between all players
    Vector3 GetPlayersMidpoint()
    {
        Vector3 midpoint = new Vector3();
        int activeCount = 0;
        
        foreach(GameObject player in players)
        {
            if (player.activeSelf)
            {
                midpoint += player.transform.position;
                activeCount++;
            }
        }

        if (activeCount != 0)
            return midpoint / activeCount;
        else
            return new Vector3();
    }

    //Get the distance from the minimum to maximum point
    float GetDistance()
    {
        Vector2[] pointArray = new Vector2[2];  //Min position, max position
        bool gotFirstPoint = false;

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                if (!gotFirstPoint)
                {
                    pointArray[0] = new Vector2();
                    pointArray[1] = new Vector2();

                    pointArray[0].x = player.transform.position.x;
                    pointArray[1].x = player.transform.position.x;
                    pointArray[0].y = player.transform.position.y;
                    pointArray[1].y = player.transform.position.y;

                    gotFirstPoint = true;
                }
                else
                {
                    pointArray[0].x = Mathf.Min(pointArray[0].x, player.transform.position.x);
                    pointArray[1].x = Mathf.Max(pointArray[1].x, player.transform.position.x);
                    pointArray[0].y = Mathf.Min(pointArray[0].y, player.transform.position.y);
                    pointArray[1].y = Mathf.Max(pointArray[1].y, player.transform.position.y);
                }

            }
        }

        return (pointArray[1] - pointArray[0]).magnitude;

    }

    int ActivePlayerCount()
    {
        int active = 0;
        foreach (GameObject player in players)
            if (player.activeSelf && player.GetComponent<PlayerInput>().GetJoystick()!= -1)
                active++;
        return active;
    }

    public void RemovePlayerFromFocus(GameObject p)
    {
        if (players.Contains(p))
            players.Remove(p);
    }

    public void AddPlayerToFocus(GameObject p)
    {
        if (!players.Contains(p))
            players.Add(p);
    }

    public void ActivateCutsceneMode()
    {
        forcingMovement = true;
    }

    public void DeactivateCutsceneMode()
    {
        forcingMovement = false;
    }
}
