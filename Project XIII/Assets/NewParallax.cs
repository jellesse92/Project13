using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewParallax : MonoBehaviour
{
    const float VIEW_ZONE = 10f;

    public bool infiniteScrolling;

    public bool parallaxXAxis;
    public bool parallaxYAxis;

    public float parallaxXSpeed;
    public float parallaxYSpeed;

    public bool autoScroll;
    public float autoScrollSpeed;

    Transform cameraTransform;
    Transform[] layers;

    float backgroundSize;

    float lastCameraX;
    float deltaX;

    float lastCameraY;
    float deltaY;

    int leftIndex;
    int rightIndex;
    Vector3 newPosition;

    void Start()
    {        
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;

        layers = new Transform[3];
        leftIndex = 0;
        rightIndex = 2;

        SetBackground();          
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (autoScroll)
        {
            foreach (var layer in layers)
                layer.position += Vector3.right * (Time.deltaTime * autoScrollSpeed);
        }

        if (parallaxXAxis)
        {
            deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxXSpeed);
            lastCameraX = cameraTransform.position.x;

        }

        if (parallaxYAxis)
        {
            deltaY = cameraTransform.position.y - lastCameraY;
            transform.position += Vector3.up * (deltaY * parallaxYSpeed);
            lastCameraY = cameraTransform.position.y;
        }

        if (infiniteScrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + VIEW_ZONE))
                ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - VIEW_ZONE))
                ScrollRight();
        }
    }

    void SetBackground()
    {
        layers[1] = transform.GetChild(0);
        backgroundSize = layers[1].GetComponent<Renderer>().bounds.size.x;

        Vector3 leftPosition = layers[1].position;
        Vector3 centerPosition = layers[1].position;
        Vector3 rightPosition = layers[1].position;

        leftPosition.x -= backgroundSize;
        rightPosition.x += backgroundSize;

        layers[0] = Instantiate(layers[1]).transform;
        layers[2] = Instantiate(layers[1]).transform;

        layers[0].transform.parent = layers[1].transform.parent;
        layers[2].transform.parent = layers[1].transform.parent;

        layers[0].transform.localScale = layers[1].transform.localScale;
        layers[2].transform.localScale = layers[1].transform.localScale;


        layers[0].transform.position = leftPosition;
        layers[1].transform.position = centerPosition;
        layers[2].transform.position = rightPosition;
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
