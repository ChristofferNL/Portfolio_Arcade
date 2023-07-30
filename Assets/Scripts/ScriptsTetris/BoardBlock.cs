using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBlock : MonoBehaviour
{
    public bool isPartOfBoard;
    public bool containsActiveCube;
    public Material activeMat;
    public Material inheritMaterial;


    private void Awake()
    {
        activeMat = this.GetComponent<Material>();
    }
    public void InheritActiveCube()
    {
        if (containsActiveCube)
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponent<MeshRenderer>().material = inheritMaterial;
            containsActiveCube = false;
            isPartOfBoard = true;
        }   
    }
    public void InheritExistingCube(BoardBlock block)
    {
        if (block.isPartOfBoard)
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponent<MeshRenderer>().material = block.GetComponent<MeshRenderer>().material;
            containsActiveCube = false;
            isPartOfBoard = true;
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<MeshRenderer>().material = null;
            containsActiveCube = false;
            isPartOfBoard = false;
        }
        
    }
    public void ClearCube()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        isPartOfBoard = false;
        containsActiveCube = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ActiveBlock")
        {
            containsActiveCube = true;
            inheritMaterial = other.gameObject.GetComponent<MeshRenderer>().material;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        containsActiveCube = false;
    }
}
