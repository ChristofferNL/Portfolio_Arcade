using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleRock : MonoBehaviour
{
    public float StartLocationOffset;
    public float TimeAlive;

    private void Update()
    {
        TimeAlive -= Time.deltaTime;
        if (TimeAlive <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void AdjustRockPosition(bool isMovingRight)
    {
        if (isMovingRight)
        {
            this.transform.position = new Vector3(this.transform.position.x + StartLocationOffset, this.transform.position.y, this.transform.position.z);
        }
        else if (!isMovingRight)
        {
            this.transform.position = new Vector3(this.transform.position.x - StartLocationOffset, this.transform.position.y, this.transform.position.z);
        }
    }

}
