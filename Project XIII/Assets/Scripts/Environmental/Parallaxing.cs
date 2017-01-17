using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {
    const float VIEW_ZONE = 10f;

    public bool infiniteScrolling;

    public bool parallaxXAxis;
    public bool parallaxYAxis;

    public float parallaxSpeed;

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
        if (name.Contains("(Clone)"))
            Destroy(GetComponent<Parallaxing>());
        else
        {
            cameraTransform = Camera.main.transform;
            lastCameraX = cameraTransform.position.x;
            lastCameraY = cameraTransform.position.y;

            layers = new Transform[3];
            leftIndex = 0;
            rightIndex = 2;

            SetBackground();

            foreach (var comp in gameObject.GetComponents<Component>())
            {
                if (!(comp is Transform || comp is Parallaxing))
                    Destroy(comp);
            }
        }
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
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
            lastCameraX = cameraTransform.position.x;

        }

        if (parallaxYAxis)
        {
            deltaY = cameraTransform.position.y - lastCameraY;
            transform.position += Vector3.up * (deltaY * parallaxSpeed);
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
