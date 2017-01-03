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
        if (name.Contains("(Clone)"))
            Destroy(GetComponent<Parallaxing>());
        else
        {
            cameraTransform = Camera.main.transform;
            lastCameraX = cameraTransform.position.x;
            layers = new Transform[3];

            SetBackground();

            leftIndex = 0;
            rightIndex = 2;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (autoScroll)
        {
            layers[0].position += Vector3.right * (Time.deltaTime * autoScrollSpeed);
            layers[1].position += Vector3.right * (Time.deltaTime * autoScrollSpeed);
            layers[2].position += Vector3.right * (Time.deltaTime * autoScrollSpeed);

        }
        if (parallax)
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
        backgroundSize = GetComponent<Renderer>().bounds.size.x;

        Vector3 leftPosition = transform.position;
        Vector3 centerPosition = transform.position;
        Vector3 rightPosition = transform.position;

        leftPosition.x -= backgroundSize;
        rightPosition.x += backgroundSize;

        layers[0] = Instantiate(gameObject).transform;
        layers[1] = Instantiate(gameObject).transform;
        layers[2] = Instantiate(gameObject).transform;

        layers[0].transform.parent = transform;
        layers[1].transform.parent = transform;
        layers[2].transform.parent = transform;

        layers[0].transform.position = leftPosition;
        layers[1].transform.position = centerPosition;
        layers[2].transform.position = rightPosition;

        foreach (var comp in gameObject.GetComponents<Component>())
        {
            if (!(comp is Transform || comp is Parallaxing))
            {
                Destroy(comp);
            }
        }
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
