using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Piece[] BlockArray = new Piece[7];
    public Piece[] FutureBlockArray = new Piece[3];
    public Piece[] FutureBlockObjects = new Piece[3];
    public Vector3[] FutureBlockSpawningLocations = new Vector3[3];
    public Vector3 ActiveBlockSpawningLocation;
    void Start()
    {
        for (int i = 0; i < FutureBlockArray.Length; i++)
        {
            FutureBlockArray[i] = AddBlockToStack();
        }
        CreateBlocks();
    }

    void Update()
    {

    }

    public Piece SpawnActiveBlock()
    {
        Piece piece;
        if (FutureBlockArray[0] == BlockArray[0] || FutureBlockArray[0] == BlockArray[3])
        {
            piece = Instantiate(FutureBlockArray[0], new Vector3(ActiveBlockSpawningLocation.x -0.5f, ActiveBlockSpawningLocation.y, ActiveBlockSpawningLocation.z -0.5f), Quaternion.identity);
        }
        else
        {
            piece = Instantiate(FutureBlockArray[0], ActiveBlockSpawningLocation, Quaternion.identity);
        }
        UpdateBlocks();
        return piece;
    }
    public void UpdateBlocks()
    {
        for (int i = 0; i < FutureBlockObjects.Length; i++)
        {
            Destroy(FutureBlockObjects[i].gameObject);
        }
        FutureBlockArray[0] = FutureBlockArray[1];
        FutureBlockArray[1] = FutureBlockArray[2];
        FutureBlockArray[2] = AddBlockToStack();
        CreateBlocks();
    }
    public void CreateBlocks()
    {
        for (int i = 0; i < FutureBlockArray.Length; i++)
        {
            FutureBlockObjects[i] = Instantiate(FutureBlockArray[i], FutureBlockSpawningLocations[i], Quaternion.identity);
        }
    }

    public Piece AddBlockToStack()
    {
        //return BlockArray[0];
        return BlockArray[Random.Range(0, 7)];
    }
}
