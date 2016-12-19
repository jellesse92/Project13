using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerCamFollowScript : MonoBehaviour {

    const float DEFAULT_ORTHO_SIZE = 7f;
    const float DEFAULT_ORTHO_SIZE_3D = 20f;
    const float MAX_ORTHO_SIZE = 10f;
    const float MAX_ORTHO_SIZE_3D = 40f;

    const float ZOOM_DELTA = .005f;                             //Amount to zoom when zooming in or out per update

    float zoomMultiplier = 1f;
    float followDelay = .8f;

    HashSet<GameObject> players = new HashSet<GameObject>();

    public bool in2DMode = true;
    public float lowestPointY = 0f;                             //Lowest point of the map the camera should be able to show
    float lastOrthographicSize = 0f;                            //Keeps track of the last orthographic size
    bool orthoTransitioning = false;
    Camera cam;

    Vector3 velocity = Vector3.zero;
    public float dampTime = 0.15f;

    // Use this for initialization
    void Start () {
        GameObject[] tempPlayerFind = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in tempPlayerFind)
        {
            players.Add(p);
        }
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        int activePlayers = ActivePlayerCount();
        if (activePlayers == 1)
            SinglePlayerCamera();
        if (activePlayers > 1)
            MultiplayerCamera();
	}

    void SinglePlayerCamera()
    {
        Transform singlePlayerTrans = transform;

        SetOrthographicSize(0);

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                singlePlayerTrans = player.transform;
                break;
            }
        }

        Vector3 point = cam.WorldToViewportPoint(singlePlayerTrans.position);
        Vector3 delta = singlePlayerTrans.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        /*
        Vector3 destination = new Vector3();
        destination.x = singlePlayerTrans.position.x;

        if (in2DMode)
            destination.y = singlePlayerTrans.position.y + 2f;
        else
            destination.y = singlePlayerTrans.position.y;

        destination.z = transform.position.z;

        transform.position = Vector3.Slerp(transform.position, destination, followDelay);

        if ((destination - transform.position).magnitude <= 0.05f)
            transform.position = destination;
        */
    }

    void MultiplayerCamera()
    {
        Vector3 midpoint = GetPlayersMidpoint();

        if (midpoint != new Vector3())
        {
            float distance = GetDistance();
            distance = Mathf.Max(1f, distance);

            Vector3 cameraDestination;

            cameraDestination = midpoint - transform.forward * distance * zoomMultiplier;
            SetOrthographicSize(distance);

            if(in2DMode)
                cameraDestination.y = Mathf.Max(lowestPointY + cam.orthographicSize-5, cameraDestination.y);
                    
            transform.position = Vector3.Slerp(transform.position, cameraDestination, followDelay);

            if ((cameraDestination - transform.position).magnitude <= 0.05f)
                transform.position = cameraDestination;
        }
    }

    //Sets orthographic size of camera based on given parameter f
    void SetOrthographicSize(float f)
    {
        float size;

        if (in2DMode)
        {
            size = Mathf.Max(f, DEFAULT_ORTHO_SIZE);
            size = Mathf.Min(size, MAX_ORTHO_SIZE);

            if(lastOrthographicSize <= size)
            {
                StopCoroutine(SmoothOrthograpicTransition(lastOrthographicSize));
                lastOrthographicSize = 0f;
                orthoTransitioning = false;
            }

            if ((cam.orthographicSize > size))
                StartCoroutine(SmoothOrthograpicTransition(size) );
            else if(!orthoTransitioning)
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

            if (cam.orthographicSize - size < ZOOM_DELTA)
            {
                cam.orthographicSize = size;
                orthoTransitioning = false;
                break;
            }

            cam.orthographicSize -= ZOOM_DELTA;
        
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
            if (player.activeSelf)
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
}
