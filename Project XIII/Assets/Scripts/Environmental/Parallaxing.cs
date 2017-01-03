using UnityEngine;
using System.Collections;


public class Parallaxing : MonoBehaviour {
    public bool scrolling;
    public bool parallax;
    public bool autoScroll;

    public float autoScrollSpeed;
    public float parallaxSpeed;

    Transform cameraTransform;
    Transform[] layers;


    float lastCameraX;
    float deltaX;

    float viewZone = 10;
    int leftIndex;
    int rightIndex;
    Vector3 newPosition;
    float backgroundSize;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];

        SetBackground();

        leftIndex = 0;
        rightIndex = layers.Length - 1;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(autoScroll)
        {
            transform.position += Vector3.right * (Time.deltaTime * autoScrollSpeed);
        }
        else if (parallax)
        {
            deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }
        lastCameraX = cameraTransform.position.x;

        if (scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }
    }

    void SetBackground()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        backgroundSize = layers[0].GetComponent<Renderer>().bounds.size.x;

        Vector3 leftPosition = layers[1].transform.position;
        Vector3 rightPosition = layers[1].transform.position;
        leftPosition.x -= backgroundSize;
        rightPosition.x += backgroundSize;

        layers[0].transform.position = leftPosition;
        layers[layers.Length - 1].transform.position = rightPosition;
    }

    private void ScrollLeft()
    {
        backgroundSize = layers[leftIndex].GetComponent<Renderer>().bounds.size.x;
        newPosition = layers[leftIndex].position;
        newPosition.x -= backgroundSize;
        layers[rightIndex].position = newPosition;

        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    private void ScrollRight()
    {
        backgroundSize = layers[leftIndex].GetComponent<Renderer>().bounds.size.x;
        newPosition = layers[rightIndex].position;
        newPosition.x += backgroundSize;
        layers[leftIndex].position = newPosition;

        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
