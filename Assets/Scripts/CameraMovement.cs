using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Range(1, 10)] public int cameraSpeedDivider = 10;

    public Vector2 baseScreenScale = new Vector2(1920, 1080);

    public Vector2 mapSize;

    float cameraSpeed = 1;
    public float minCamSize = 1, maxCamSize = 10;
    public Transform objectToFollow = null;

    Camera cam;
    Vector2 mousePosDelta;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        float moveX = 0, moveY = 0;
        cameraSpeed = (maxCamSize / cam.orthographicSize) * cameraSpeedDivider * 10;

        if (Input.GetMouseButton(1))
        {
            moveX = (mousePosDelta.x - Input.mousePosition.x) / cameraSpeed * (baseScreenScale.x / Screen.width);
            moveY = (mousePosDelta.y - Input.mousePosition.y) / cameraSpeed * (baseScreenScale.y / Screen.height);
        }

        //float camScaler = 1 / -(1 + maxCamSize - minCamSize) * cam.orthographicSize + 2;

        /*
            10 =    4.5;   7,2
            9 =     4;      6.4
            8 =     3.5;    5.6
            7 =     3;      4.8
            6 =     2.5;    4
            5 =     2;      3.2
            4 =     1.5;    2.4
            3 =     1;      1,6
            2 =     0.5;    0,8
            1 =     0;      0
         
         */


        float xOffset = (1 + cam.orthographicSize - maxCamSize) * 1.6f / 2 + ((cam.orthographicSize - 1) * .5f * 1.6f);
        float yOffset = (1 + cam.orthographicSize - maxCamSize) / 2 + ((cam.orthographicSize - 1) * .5f);

        if (!objectToFollow) 
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + moveX, xOffset, mapSize.x - xOffset),
                Mathf.Clamp(transform.position.y + moveY, yOffset, mapSize.y - yOffset),
                transform.position.z);
        else
            transform.position = new Vector3(
                    Mathf.Clamp(objectToFollow.position.x + moveX, xOffset, mapSize.x - xOffset),
                    Mathf.Clamp(objectToFollow.position.y + moveY, yOffset, mapSize.y - yOffset),
                    transform.position.z);

        mousePosDelta = Input.mousePosition;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y, minCamSize, maxCamSize);
    }



    void OnDrawGizmos()
    {
        //position
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(mapSize.x / 2, mapSize.y / 2, 0), new Vector3(mapSize.x, mapSize.y, 0));
        //vision
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(mapSize.x / 2, mapSize.y / 2, 0), new Vector3(mapSize.x, mapSize.y, 0));
    }
}
