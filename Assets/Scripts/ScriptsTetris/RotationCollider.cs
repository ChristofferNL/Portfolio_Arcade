using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCollider : MonoBehaviour
{
    public bool IsCollidingWithMesh;

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<MeshRenderer>().enabled)
        {
            IsCollidingWithMesh = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MeshRenderer>().enabled)
        {
            IsCollidingWithMesh = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsCollidingWithMesh = false;
    }
}
