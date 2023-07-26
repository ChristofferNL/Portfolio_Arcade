using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Simple camera follow script
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private float offsetZ;
    private Camera camera;
    [SerializeField] private Transform leftBounds;
    [SerializeField] private Transform rightBounds;
    [SerializeField] private Transform lowerBounds;
    [SerializeField] private Transform upperBounds;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
       
    }

    private void Start() // set the FOV depending on your screen size
    {
        if (Screen.width / Screen.height < 2)
        {
            camera.fieldOfView = 60;
        }
        else
        {
            camera.fieldOfView = 55;
        }
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer() // move the camera position using a Lerp, and locking the screen inside the bounds of the map
    {
        camera.transform.position = new Vector3(Mathf.Lerp(camera.transform.position.x, transform.position.x, followSpeed), Mathf.Lerp(camera.transform.position.y, transform.position.y, followSpeed), offsetZ);
        if (camera.transform.position.x + offsetZ < leftBounds.position.x)
        {
            camera.transform.position = new Vector3(leftBounds.position.x - offsetZ, camera.transform.position.y, offsetZ);
        }
        if (camera.transform.position.x - offsetZ > rightBounds.position.x)
        {
            camera.transform.position = new Vector3(rightBounds.position.x + offsetZ, camera.transform.position.y, offsetZ);
        }
        if (camera.transform.position.y + offsetZ / 2 < lowerBounds.position.y)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, lowerBounds.position.y - offsetZ / 2, offsetZ);
        }
        if (camera.transform.position.y - offsetZ / 2 > upperBounds.position.y)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, upperBounds.position.y + offsetZ / 2, offsetZ);
        }
    }
}
