using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Makes the camera smoothly follow the GameObject with this script attatched to it, must declare a camera in the inspector
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Camera MyCamera;
    public float OffsetZ;
    public float OffsetY;
    public float FollowTime;

    private void Update()
    {
        MyCamera.transform.position = new Vector3(Mathf.Lerp(MyCamera.transform.position.x, transform.position.x, FollowTime * Time.deltaTime),
        Mathf.Lerp(MyCamera.transform.position.y, transform.position.y + OffsetY, FollowTime * Time.deltaTime), 
        Mathf.Lerp(MyCamera.transform.position.z, transform.position.z + OffsetZ, FollowTime * Time.deltaTime));
    }
}
