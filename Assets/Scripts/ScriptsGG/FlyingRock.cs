using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRock : MonoBehaviour
{

    public int Angle;
    public float Speed;
    public float StartLocationOffset;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SendRockFlying(bool isMovingRight, float charVelocity)
    {
        if (charVelocity > 0)
        {
            Speed += charVelocity;
        }
        else
        {
            Speed -= charVelocity;
        }
       
        if (isMovingRight)
        {
            this.transform.position = new Vector3(this.transform.position.x + StartLocationOffset, this.transform.position.y, this.transform.position.z);
            rigidbody.AddForce(new Vector3(1, 0.5f, 0) * Speed , ForceMode.Impulse);
        }
        else if (!isMovingRight)
        {
            this.transform.position = new Vector3(this.transform.position.x - StartLocationOffset, this.transform.position.y, this.transform.position.z);
            rigidbody.AddForce(new Vector3(-1, 0.5f, 0) * Speed, ForceMode.Impulse);
        }
        
    }

}
